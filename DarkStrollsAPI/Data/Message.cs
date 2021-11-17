using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data
{
    /// <summary>
    /// Message in the database.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Unique Id of the message.
        /// </summary>
        [Key]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        /// <summary>
        /// User that created the message.
        /// </summary>
        [Column("userid", TypeName = "int")]
        public int UserId { get; set; }

        /// <summary>
        /// Text of the message.
        /// </summary>
        [Column("text", TypeName = "nvarchar(255)")]
        public string Text { get; set; } = "";

        /// <summary>
        /// X Location of the message.
        /// </summary>
        [Column("locationx", TypeName = "DOUBLE")]
        public double? Longitude { get; set; }

        /// <summary>
        /// Y Location of the message.
        /// </summary>
        [Column("locationy", TypeName = "DOUBLE")]
        public double? Latitude { get; set; }
        
    }
}
