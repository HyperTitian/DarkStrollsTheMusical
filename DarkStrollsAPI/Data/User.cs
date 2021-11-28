using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data
{
    /// <summary>
    /// A User in the database.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique Id of the user.
        /// </summary>
        [Key]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        /// <summary>
        /// Soul count for the user.
        /// </summary>
        [Column("souls", TypeName = "int")]
        public int Souls { get; set; }

        /// <summary>
        /// Username of the user.
        /// </summary>
        [Column("username", TypeName = "nvarchar(255)")]
        public string Username { get; set; } = "";

        /// <summary>
        /// Password hash for the user.
        /// </summary>
        [Column("saltbits", TypeName = "BINARY(48)")]
        public byte[]? SaltHash { get; set; }

        /// <summary>
        /// The session id of the user.
        /// </summary>
        //[Column("sessionId", TypeName = "BINARY(32)")]
        //public byte[]? SessionId { get; set; }

        //[Column("sessionExpires", TypeName = "datetime2(7)")]
       // public DateTime? SessionExpires { get; set; }
    }
}
