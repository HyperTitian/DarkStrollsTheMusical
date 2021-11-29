using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data.Requests
{
    /// <summary>
    /// Format for GetMessages request.
    /// </summary>
    public class GetMessagesRequest
    {
        /// <summary>
        /// Ids of requested messages.
        /// </summary>
        public int[]? MessageIds { get; set; }

        /// <summary>
        /// User ids of the creators of requested messages.
        /// </summary>
        public int[]? UserIds { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public double? Range { get; set; }

        /// <summary>
        /// Usernames of the creators of requested messages.
        /// </summary>
        public string[]? Usernames { get; set; }

        /// <summary>
        /// Whether the result set should include the usernames.
        /// </summary>
        public bool? IncludeUsernames { get; set; } = false;
    }
}
