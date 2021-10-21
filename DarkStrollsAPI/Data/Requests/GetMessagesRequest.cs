using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data.Requests
{
    public class GetMessagesRequest
    {
        public int[]? MessageIds { get; set; }

        public int[]? UserIds { get; set; }

        public string[]? Usernames { get; set; }

        public bool? IncludeUsernames { get; set; } = false;
    }
}
