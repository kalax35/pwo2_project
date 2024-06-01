﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PWO.Client.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                var apiAccessToken = Session["ApiAccessToken"] as string;
                if (!string.IsNullOrEmpty(apiAccessToken))
                {
                    return RedirectToAction("Index", "ToDoList");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}