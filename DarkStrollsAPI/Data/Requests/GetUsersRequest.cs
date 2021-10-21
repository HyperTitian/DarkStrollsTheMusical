using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data.Requests
{
    public class GetUsersRequest
    {
        public int[]? UserIds { get; set; }

        public string[]? Usernames { get; set; }
    }
}
