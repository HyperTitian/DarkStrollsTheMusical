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
    /// Controller to handle creating a bonfire.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CreateBonfireController : ControllerBase
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
            var request = JsonConvert.DeserializeObject<CreateBonfireRequest>(body);

            // Check the object was formed properly.
            if(request is null)
            {
                return "Malformed request!";
            }
            else if (request.Longitude is null)
            {
                return "Must include Longitude!";
            }
            else if (request.Latitude is null)
            {
                return "Must include Latitude!";
            }
            else if (request.Name is null)
            {
                return "Must include Name!";
            }

            // Create the context.
            var dbContext = new DarkDbContext();

            // Create the bonfire and set the name and location.
            Bonfire bonfire = new Bonfire();
            bonfire.Name = request.Name;
            bonfire.Latitude = request.Latitude.Value;
            bonfire.Longitude = request.Longitude.Value;

            // Check the bonfire doesn't exist yet.
            if(dbContext.Bonfires.Where(x => x.Latitude == request.Latitude && x.Longitude == request.Longitude).Any())
            {
                await dbContext.DisposeAsync();
                return "There is already a bonfire at that location!";
            }

            // Add the bonfire to the context, save, and dispose.
            dbContext.Add(bonfire);
            await dbContext.SaveChangesAsync();
            await dbContext.DisposeAsync();

            // Return the bonfire as a JSON object.
            return JsonConvert.SerializeObject(bonfire);
        }
    }
}
