using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data.Requests
{
    public class CreateBonfireRequest
    {
        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public string? Name { get; set; }
    }
}
