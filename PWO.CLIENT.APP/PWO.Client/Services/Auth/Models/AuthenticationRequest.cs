using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PWO.Client.Services.Auth.Models
{
    public class AuthenticationRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

    }
}