using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data
{
    public class Message
    {
        [Key]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        [Column("userid", TypeName = "int")]
        public int UserId { get; set; }

        [Column("text", TypeName = "nvarchar(255)")]
        public string Text { get; set; } = "";
        
    }
}
