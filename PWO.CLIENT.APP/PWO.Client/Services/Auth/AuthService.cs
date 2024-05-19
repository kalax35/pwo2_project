using PWO.Client.Services.Auth.Models;
using PWO.Client.Services.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PWO.Client.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly RequestService _requestService;

        public AuthService(RequestService requestService)
        {
            _requestService = requestService;
        }


        public async Task<bool> LoginAsync(string login, string password, string token)
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}api/auth/login";

                var request = new AuthenticationRequest()
                {
                    Password = password,
                    Email = login
                };

                var response = await _requestService.PostAsync<AuthenticationRequest, AuthenticationResponse>(uri, request);

                _requestService.ClearHttpConfig();
                return true;
            }
            catch (System.Exception ex)
            {

                throw;
            }
            return false;
        }


        public void LogOut()
        {
            //AppSettings.RemoveUserData();
            //AppSettings.UserLoggedIn = false;
        }

    }
}