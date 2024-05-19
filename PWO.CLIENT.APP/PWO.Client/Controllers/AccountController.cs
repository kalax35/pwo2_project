using Newtonsoft.Json;
using PWO.Client.Services.Auth.Models;
using PWO.Client.Services.Auth;
using PWO.Client.Services.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PWO.Client.Controllers
{
    public class AccountController : Controller
    {

        private readonly RequestService _requestService;

        public AccountController(RequestService requestService)
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
                    UserName = login,
                    Token = token,
                };

                var response = await _requestService.PostAsync<AuthenticationRequest, AuthenticationResponse>(uri, request);

                Session["ApiAccessToken"] = response.JWTToken;
                Session["TokenExpiration"] = response.ExpiryDate;
                Session["UserId"] = response.UserId;
                Session["UserName"] = response.UserName;

                _requestService.ClearHttpConfig();
                return true;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
    }
}