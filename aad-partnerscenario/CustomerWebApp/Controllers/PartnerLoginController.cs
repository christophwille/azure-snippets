using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;

namespace CustomerWebApp.Controllers
{
    public class PartnerLoginController : Controller
    {
        public ActionResult SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/PartnerLogin/Welcome" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);


                return new EmptyResult();
            }

            return Welcome();
        }

        public ActionResult Welcome()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}