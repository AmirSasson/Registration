using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWebApi.Models
{
    public class SegmentToken
    {
        public bool Authenticated { get; internal set; }
        public int EntityId { get; internal set; }
        public string Token { get; internal set; }
        public DateTime TokenExpires { get; internal set; }
    }
}
