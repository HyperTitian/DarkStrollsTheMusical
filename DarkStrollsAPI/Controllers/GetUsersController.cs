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
    /// Controller to handle getting users.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class GetUsersController : ControllerBase
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
            // Convert the request to a string.
            string body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            // Parse the JSON string to a C# object.
            var request = JsonConvert.DeserializeObject<GetUsersRequest>(body);

            // Check that it was parsed ok.
            if(request is null)
            {
                // It wasn't, so throw an error.
                return "Malformed request!";
            }

            // Create the database context.
            var dbContext = new DarkDbContext();

            // Create a list to store our users.
            var users = new HashSet<User>();

            // Get all users matching the given user ids.
            if(request.UserIds != null)
            {
                users.UnionWith(await dbContext.Users.Where(x => request.UserIds.Contains(x.Id)).ToListAsync());
            }

            // Get all users matching the given usernames.
            if(request.Usernames != null)
            {
                users.UnionWith(await dbContext.Users.Where(x => request.Usernames.Contains(x.Username)).ToListAsync());
            }

            // Dispose of the database.
            await dbContext.DisposeAsync();

            // Return the array as a JSON object.
            return JsonConvert.SerializeObject(users.ToArray());
        }
    }
}
