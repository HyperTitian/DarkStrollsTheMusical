using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data.Requests
{
    public class CreateMessageRequest
    {
        public int UserId { get; set; }

        public string? Username { get; set; }

        public string Text = "";
    }
}
