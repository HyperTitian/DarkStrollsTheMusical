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
    public class GetBonfiresController : ControllerBase
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
            var request = JsonConvert.DeserializeObject<GetBonfiresRequest>(body);

            // Verify the object was created correctly.
            if(request is null)
            {
                // It wasn't, so return an error.
                return "Malformed request!";
            }
            else if(request.Longitude is null)
            {
                return "Must contain longitude!";
            }
            else if(request.Latitude is null)
            {
                return "Must contain latitude!";
            }
            else if(request.Range is null)
            {
                return "Must contain range!";
            }

            // Create the context.
            var dbContext = new DarkDbContext();

            // Get the bonfires.
            List<Bonfire> bonfires = await dbContext.Bonfires.Where(x => x.Longitude < request.Longitude + request.Range)
                                                             .Where(x => x.Longitude > request.Longitude - request.Range)
                                                             .Where(x => x.Latitude < request.Latitude + request.Range)
                                                             .Where(x => x.Latitude > request.Latitude - request.Range).ToListAsync();

            string result = JsonConvert.SerializeObject(bonfires);

            // Dispose of the database context.
            await dbContext.DisposeAsync();

            // Return our result.
            return result ?? "";
        }
    }
}
