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
    public class CreateUserController : ControllerBase
    {

        [HttpGet]
        public async Task<string> Get()
        {
            return await PostAsync();
        }

        public async Task<string> PostAsync()
        {
            string body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<CreateUserRequest>(body);

            if(request is null)
            {
                return "Malformed request!";
            }


            var dbContext = new DarkDbContext();

            bool exists = await dbContext.Users.AnyAsync(x => x.Username == request.Username);

            if(exists)
            {
                await dbContext.DisposeAsync();
                return "User exists!";
            }

            var user = new User()
            {
                Username = request.Username
            };

            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            await dbContext.DisposeAsync();

            return JsonConvert.SerializeObject(user);
        }
    }
}
