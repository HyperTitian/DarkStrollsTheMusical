using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data.Requests
{
    /// <summary>
    /// Format for GetUsers request.
    /// </summary>
    public class GetUsersRequest
    {
        /// <summary>
        /// UserIds requested.
        /// </summary>
        public int[]? UserIds { get; set; }

        /// <summary>
        /// Usernames requested.
        /// </summary>
        public string[]? Usernames { get; set; }
    }
}
