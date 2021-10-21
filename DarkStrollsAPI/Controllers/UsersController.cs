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
using DarkStrollsAPI.Data.Requests;

namespace DarkStrollsAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public UsersController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return JsonConvert.SerializeObject(new MessageRequest());
        }

        public async Task<JsonResult> PostAsync()
        {
            string body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var mine = JsonConvert.DeserializeObject<MessageRequest>(body);
            
            //var returnValue = new JObject();
            //returnValue.Add("Password", "s");

            return new JsonResult(mine?.MessageId ?? 0);
        }
    }
}
