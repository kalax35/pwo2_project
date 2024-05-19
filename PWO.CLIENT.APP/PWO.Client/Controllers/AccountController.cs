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
using System.Security.Policy;

namespace PWO.Client.Controllers
{
    public class AccountController : Controller
    {

        private readonly RequestService _requestService;

        public AccountController(RequestService requestService)
        {
            _requestService = requestService;
        }

        public ActionResult Login()
        {
            return View();
        }

        public async Task<RedirectToRouteResult> LoginAsync(AuthenticationRequest model)
        {
            try
            {
                var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/login?useCookies=false&useSessionCookies=false";

                var request = new
                {
                    email = model.Email,
                    password = model.Password
                };

                var response = await _requestService.PostAsync<object, AuthenticationResponse>(uri, request);

                Session["ApiAccessToken"] = response.AccessToken;
                Session["TokenExpiration"] = DateTime.UtcNow.AddSeconds(response.ExpiresIn);


                _requestService.ClearHttpConfig();
                return RedirectToAction("Index", "Home");
                //return true;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(AuthenticationRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var uri = $"{ConfigurationManager.AppSettings["ApiBaseUrl"]}/register";

            var request = new
            {
                email = model.Email,
                password = model.Password
            };

            try
            {
                var response = await _requestService.PostAsync<object>(uri, request);

                return RedirectToAction("RegisterApproved");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Registration failed: " + ex.Message);
            }

            ModelState.AddModelError("", "Registration failed.");
            return View(model);

        }

        public ActionResult RegisterApproved()
        {
            return View();
        }
    }
}