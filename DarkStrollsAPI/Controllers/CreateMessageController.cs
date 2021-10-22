using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DarkStrollsAPI.Data;
using System.IO;
using DarkStrollsAPI.Models;
using Microsoft.EntityFrameworkCore;
using DarkStrollsAPI.Data.Requests;

namespace DarkStrollsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreateMessageController : ControllerBase
    {

        [HttpGet]
        public async Task<string> Get()
        {
            return await PostAsync();
        }

        public async Task<string> PostAsync()
        {
            string body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<CreateMessageRequest>(body);

            if(request is null)
            {
                return "Malformed request!";
            }


            var dbContext = new DarkDbContext();
            Message message = new Message();
            message.Text = request.Text;

            if(request.Username is null)
            {
                bool exists = await dbContext.Users.AnyAsync(x => x.Id == request.UserId);

                if(!exists)
                {
                    await dbContext.DisposeAsync();
                    return "That user doesn't exist!";
                }

                message.UserId = request.UserId;
            }
            else
            {
                User user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username);

                if (user is null)
                {
                    await dbContext.DisposeAsync();
                    return "That user doesn't exist!";
                }

                message.UserId = user.Id;
            }

            dbContext.Add(message);
            await dbContext.SaveChangesAsync();
            await dbContext.DisposeAsync();

            return JsonConvert.SerializeObject(message);
        }
    }
}
