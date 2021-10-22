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
    [ApiController]
    [Route("[controller]")]
    public class GetMessagesController : ControllerBase
    {

        [HttpGet]
        public async Task<string> Get()
        {
            return await PostAsync();
        }

        public async Task<string> PostAsync()
        {
            string body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var request = JsonConvert.DeserializeObject<GetMessagesRequest>(body);

            if(request is null)
            {
                return "Malformed request!";
            }


            var dbContext = new DarkDbContext();

            var messageIds = new HashSet<int>();

            if(request.UserIds != null)
            {
                messageIds.UnionWith(await dbContext.Messages.Where(x => request.UserIds.Contains(x.UserId)).Select(x => x.Id).ToListAsync());
            }

            if(request.Usernames != null)
            {
                var userIds = await dbContext.Users.Where(x => request.Usernames.Contains(x.Username)).Select(x => x.Id).ToListAsync();
                messageIds.UnionWith(await dbContext.Messages.Where(x => userIds.Contains(x.UserId)).Select(x => x.Id).ToListAsync());
            }

            if(request.MessageIds != null)
            {
                messageIds.UnionWith(request.MessageIds);
            }

            string? result = null;

            if(request.IncludeUsernames ?? false)
            {
                var messages = await dbContext.Messages.Where(x => messageIds.Contains(x.Id)).Select(x => new GetMessageResponse() { MessageId = x.Id,
                                                                                                                                     MessageText = x.Text,
                                                                                                                                     UserId = x.UserId }).ToListAsync();
                int[] userIds = messages.Select(x => x.UserId).ToHashSet().ToArray();
                var users = await dbContext.Users.Where(x => userIds.Contains(x.Id)).Select(x => new { x.Username, x.Id }).ToDictionaryAsync(x => x.Id);
                foreach(var message in messages)
                {
                    message.Username = users[message.UserId].Username;
                }
                result = JsonConvert.SerializeObject(messages);
            }
            else
            {
                var messages = await dbContext.Messages.Where(x => messageIds.Contains(x.Id)).ToArrayAsync();
                result = JsonConvert.SerializeObject(messages);
            }

            await dbContext.DisposeAsync();

            return result ?? "";
        }
    }
}
