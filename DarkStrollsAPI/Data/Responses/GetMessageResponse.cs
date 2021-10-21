using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data.Responses
{
    public class GetMessageResponse
    {
        public int MessageId { get; set; }

        public int UserId { get; set; }

        public string? Username { get; set; }

        public string? MessageText { get; set; }
    }
}
