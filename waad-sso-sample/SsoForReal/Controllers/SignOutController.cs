using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SsoForReal.Controllers
{
    public class SignOutController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public void SignOut()
        {
            var fc = FederatedAuthentication.FederationConfiguration.WsFederationConfiguration;

            string request = System.Web.HttpContext.Current.Request.Url.ToString();
            string wreply = request.Substring(0, request.Length - "SignOut".Length);

            var soMessage = new SignOutRequestMessage(new Uri(fc.Issuer), wreply);
            soMessage.SetParameter("wtrealm", fc.Realm);

            FederatedAuthentication.SessionAuthenticationModule.SignOut();
            Response.Redirect(soMessage.WriteQueryString());
        }
    }
}
