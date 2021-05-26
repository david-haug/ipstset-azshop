using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Auth.JwtTokens
{
    public class JwtTokenSettings
    {
        public IEnumerable<string> Issuers { get; set; }
        public IEnumerable<string> Audiences { get; set; }
        public int MinutesToExpire { get; set; }
        public string Secret { get; set; }
    }
}
