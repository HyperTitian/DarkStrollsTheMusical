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
    /// Controller to handle creating a message.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CreateMessageController : ControllerBase
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

            // Convert the body to a C# object.
            var request = JsonConvert.DeserializeObject<CreateMessageRequest>(body);

            // Check the object was formed properly.
            if(request is null)
            {
                return "Malformed request!";
            }

            // Create the context.
            var dbContext = new DarkDbContext();

            // Create the message and set the text.
            Message message = new Message();
            message.Text = request.Text;

            // Check if they provided a username, otherwise it's Id.
            if(request.Username is null)
            {
                // Get whether the user exists.
                bool exists = await dbContext.Users.AnyAsync(x => x.Id == request.UserId);

                // Check the value.
                if(!exists)
                {
                    // Doesn't exist, so clean up and return an error.
                    await dbContext.DisposeAsync();
                    return "That user doesn't exist!";
                }

                // Set the message id.
                message.UserId = request.UserId;
            }
            else
            {
                // Get the user from the username.
                User user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username);

                // Check if the user exists.
                if (user is null)
                {
                    // Doesn't exist, so clean up and return an error.
                    await dbContext.DisposeAsync();
                    return "That user doesn't exist!";
                }

                // Set the user id.
                message.UserId = user.Id;
            }

            if(request.Latitude is null || request.Longitude is null)
            {
                await dbContext.DisposeAsync();
                return "You must include longitude and latitude!";
            }

            message.Longitude = request.Longitude;
            message.Latitude = request.Latitude;

            // Add the message to the context, save, and dispose.
            dbContext.Add(message);
            await dbContext.SaveChangesAsync();
            await dbContext.DisposeAsync();

            // Return the message as a JSON object.
            return JsonConvert.SerializeObject(message);
        }
    }
}
