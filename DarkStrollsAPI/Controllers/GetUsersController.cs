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
    public class GetUsersController : ControllerBase
    {

        [HttpGet]
        public async Task<string> Get()
        {
            return await PostAsync();
        }

        public async Task<string> PostAsync()
        {
            string body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<GetUsersRequest>(body);

            if(request is null)
            {
                return "Malformed request!";
            }


            var dbContext = new DarkDbContext();

            var users = new List<User>();

            if(request.UserIds != null)
            {
                users.AddRange(await dbContext.Users.Where(x => request.UserIds.Contains(x.Id)).ToListAsync());
            }

            if(request.Usernames != null)
            {
                users.AddRange(await dbContext.Users.Where(x => request.Usernames.Contains(x.Username)).ToListAsync());
            }

            await dbContext.DisposeAsync();

            return JsonConvert.SerializeObject(users);
        }
    }
}
