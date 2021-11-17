using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data.Requests
{
    /// <summary>
    /// Format for CreateMessage request.
    /// </summary>
    public class CreateMessageRequest
    {
        /// <summary>
        /// UserId that is creating the message.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Username that is creating the message.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Text to store in the message.
        /// </summary>
        public string Text = "";

        /// <summary>
        /// Longitude of the message.
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// Latitude of the message.
        /// </summary>
        public double? Latitude { get; set; }
    }
}
