using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data
{
    /// <summary>
    /// A bonfire as seen in the database.
    /// </summary>
    public class Bonfire
    {
        [Key]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        [Column("name", TypeName = "varchar(255)")]
        public string Name { get; set; } = null!;

        [Column("longitude", TypeName = "float")]
        public double Longitude { get; set; }

        [Column("latitude", TypeName = "float")]
        public double Latitude { get; set; }
    }
}
