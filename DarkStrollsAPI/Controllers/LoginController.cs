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
using DarkStrollsAPI.Data.Responses;
using DarkStrollsAPI.Security;

namespace DarkStrollsAPI.Controllers
{
    /// <summary>
    /// Controller to handle getting messages.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
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
            // Get the body as a string.
            string body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            
            // Convert the body to a JSON object.
            var request = JsonConvert.DeserializeObject<LoginRequest>(body);

            // Verify the object was created correctly.
            if(request is null)
            {
                // It wasn't, so return an error.
                return "Malformed request!";
            }

            // Create the context.
            var dbContext = new DarkDbContext();

            // Get the user.
            var user = await dbContext.Users.Where(x => x.Username == request.Username).FirstOrDefaultAsync();

            // Check the user exists.
            if(user is null || request.Password is null)
            {
                // It doesn't.
                await dbContext.DisposeAsync();
                return "Invalid Login!";
            }

            // Check the password.
            AuthenticationChecker authChecker = new AuthenticationChecker();
            if(!authChecker.CheckPassword(user, request.Password))
            {
                // Password fails.
                await dbContext.DisposeAsync();
                return "Invalid Login!";
            }

            // Dispose of the database.
            await dbContext.DisposeAsync();

            // Don't set the salthash.
            user.SaltHash = null;

            // Convert the user to json.
            var result = JsonConvert.SerializeObject(user);

            // Return our result.
            return result;
        }
    }
}
