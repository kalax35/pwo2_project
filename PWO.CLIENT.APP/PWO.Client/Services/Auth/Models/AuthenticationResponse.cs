using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PWO.Client.Services.Auth
{
    public class AuthenticationResponse
    {
        public string JWTToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}