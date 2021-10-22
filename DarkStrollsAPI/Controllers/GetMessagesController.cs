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

namespace DarkStrollsAPI.Controllers
{
    /// <summary>
    /// Controller to handle getting messages.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class GetMessagesController : ControllerBase
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
            var request = JsonConvert.DeserializeObject<GetMessagesRequest>(body);

            // Verify the object was created correctly.
            if(request is null)
            {
                // It wasn't, so return an error.
                return "Malformed request!";
            }

            // Create the context.
            var dbContext = new DarkDbContext();

            // Create a HashSet to store unique message ids.
            var messageIds = new HashSet<int>();

            // Get message ids from the listed user ids.
            if(request.UserIds != null)
            {
                messageIds.UnionWith(await dbContext.Messages.Where(x => request.UserIds.Contains(x.UserId)).Select(x => x.Id).ToListAsync());
            }

            // Get message ids from the listed usernames.
            if(request.Usernames != null)
            {
                var userIds = await dbContext.Users.Where(x => request.Usernames.Contains(x.Username)).Select(x => x.Id).ToListAsync();
                messageIds.UnionWith(await dbContext.Messages.Where(x => userIds.Contains(x.UserId)).Select(x => x.Id).ToListAsync());
            }

            // Get message ids from the listed message ids
            if(request.MessageIds != null)
            {
                messageIds.UnionWith(request.MessageIds);
            }

            // Create string variable to store the result.
            string? result = null;

            // Check whether they wanted usernames or not.
            if(request.IncludeUsernames ?? false)
            {
                // Create result set including usernames.

                // Get all messages for the ids we have and load them into GetMessageResponse objects.
                var messages = await dbContext.Messages.Where(x => messageIds.Contains(x.Id)).Select(x => new GetMessageResponse() { MessageId = x.Id,
                                                                                                                                     MessageText = x.Text,
                                                                                                                                     UserId = x.UserId }).ToListAsync();
                // Create an array for all the user ids in the messages.
                int[] userIds = messages.Select(x => x.UserId).ToHashSet().ToArray();

                // Get all usernames and ids that match our list of user ids.
                var users = await dbContext.Users.Where(x => userIds.Contains(x.Id)).Select(x => new { x.Username, x.Id }).ToDictionaryAsync(x => x.Id);
                
                // Set the username in each message.
                foreach(var message in messages)
                {
                    message.Username = users[message.UserId].Username;
                }

                // Set the result to our array converted to JSON text.
                result = JsonConvert.SerializeObject(messages);
            }
            else
            {
                // Get all messages in our id list.
                var messages = await dbContext.Messages.Where(x => messageIds.Contains(x.Id)).ToArrayAsync();

                // Set the result as a JSON array.
                result = JsonConvert.SerializeObject(messages);
            }

            // Dispose of the database context.
            await dbContext.DisposeAsync();

            // Return our result.
            return result ?? "";
        }
    }
}
