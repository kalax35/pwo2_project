using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PWO.Client.Services.Auth
{
    public class AuthenticationResponse
    {
        public string Bearer { get; set; }
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
    }
}