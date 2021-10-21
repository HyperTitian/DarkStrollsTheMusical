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

        private readonly ILogger<WeatherForecastController> _logger;

        public CreateUserController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            var dbContext = new DarkDbContext();

            bool exists = await dbContext.Users.AnyAsync(x => x.Username == "Bob");

            //var returnValue = new JObject();
            //returnValue.Add("Password", "s");

            return $"Exists: {exists} Username: {"Bob"}";
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
            user = await dbContext.Users.FirstAsync(x => x.Username == user.Username);
            await dbContext.DisposeAsync();

            //var returnValue = new JObject();
            //returnValue.Add("Password", "s");

            return JsonConvert.SerializeObject(user);
        }
    }
}
