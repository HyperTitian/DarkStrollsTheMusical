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
    /// <summary>
    /// Controller to handle creating a user.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CreateUserController : ControllerBase
    {
        /// <summary>
        /// Get Request.
        /// This should not be used, but is here for compatability. Use POST instead.
        /// </summary>
        /// <returns>JSON formatted text for a valid request.</returns>
        [HttpGet]
        public async Task<string> Get()
        {
            return await PostAsync();
        }

        /// <summary>
        /// Post Request.
        /// </summary>
        /// <returns>JSON formatted text for a valid request.</returns>
        public async Task<string> PostAsync()
        {
            // Read the user's request to a string.
            string body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            // Convert the json string to an object.
            var request = JsonConvert.DeserializeObject<CreateUserRequest>(body);

            // If the request is null, return an error.
            if(request is null)
            {
                return "Malformed request!";
            }

            // Create the database.
            var dbContext = new DarkDbContext();

            // Check whether the user exists in the database.
            bool exists = await dbContext.Users.AnyAsync(x => x.Username == request.Username);

            // If it exists, throw an error, clean up, and return.
            if(exists)
            {
                await dbContext.DisposeAsync();
                return "User exists!";
            }

            // Create the new user.
            var user = new User()
            {
                Username = request.Username
            };
            
            // Add the user to the context, save changes, and dispose.
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            await dbContext.DisposeAsync();

            // Return the new user as a JSON object.
            return JsonConvert.SerializeObject(user);
        }
    }
}
