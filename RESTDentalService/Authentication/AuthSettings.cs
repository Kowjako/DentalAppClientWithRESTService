using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTDentalService.Authentication
{
    public class AuthSettings
    {
        public string JwtKey { get; set; }
        public int JwtExpireDays { get; set; }
        public string JwtIssuer { get; set; }
    }
}
