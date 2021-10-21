using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data
{
    public class User
    {
        [Key]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        [Column("souls", TypeName = "int")]
        public int Souls { get; set; }

        [Column("username", TypeName = "nvarchar(255)")]
        public string Username { get; set; } = "";
    }
}
