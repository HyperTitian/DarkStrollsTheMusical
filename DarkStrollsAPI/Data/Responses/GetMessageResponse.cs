using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data.Responses
{
    /// <summary>
    /// Response including Username to GetMessages request.
    /// </summary>
    public class GetMessageResponse
    {
        /// <summary>
        /// Id of the message.
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// UserId of the creator of the message.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Username of the creator of the message.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Type of the message.
        /// </summary>
        public string? MessageText { get; set; }
    }
}
