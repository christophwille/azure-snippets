using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SsoForReal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.UserPrincipalName = ClaimsPrincipal.Current.Identity.Name;
            ViewBag.Name = GetFullname();

            return View();
        }

        private string GetFullname()
        {
            ClaimsPrincipal cp = ClaimsPrincipal.Current;
            string fullname =
                   string.Format("{0} {1}", cp.FindFirst(ClaimTypes.GivenName).Value,
                   cp.FindFirst(ClaimTypes.Surname).Value);

            return fullname;
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult ForAdminEyesOnly()
        {
            return View();
        }
    }
}
