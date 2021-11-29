using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkStrollsAPI.Data.Requests
{
    /// <summary>
    /// Request for getting the bonfires.
    /// </summary>
    public class GetBonfiresRequest
    {
        public float? Longitude { get; set; }

        public float? Latitude { get; set; }

        public float? Range { get; set; }
    }
}
