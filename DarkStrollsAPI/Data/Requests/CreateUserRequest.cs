using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data.Requests
{
    /// <summary>
    /// Format for CreateUser request.
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// Username of the user to be created.
        /// </summary>
        public string Username { get; set; } = "";

        public string? Password { get; set; }
    }
}
