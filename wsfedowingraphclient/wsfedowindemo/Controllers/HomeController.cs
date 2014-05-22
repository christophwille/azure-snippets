using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using wsfedowindemo.Models;

namespace wsfedowindemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        #region Default Actions
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        #endregion

        [Authorize]
        public ActionResult UserProfile()
        {
#if DEBUG
            ClaimsPrincipal.Current.DebugPrintAllClaims();
#endif
            string aadObjectIdOfUser = ClaimsPrincipal.Current.GetObjectId();

            var graphOps = new GraphOperations();
            graphOps.Initialize();

            var user = graphOps.GetUser(aadObjectIdOfUser);

            var profile = new UserProfile()
            {
                DisplayName = user.DisplayName,
                GivenName = user.GivenName,
                Surname = user.Surname
            };

            return View(profile);
        }

        [Authorize(Roles = "Demo Group A")]
        public ActionResult RoleAOnly()
        {
            return View();
        }

        [Authorize(Roles = "Demo Group B")]
        public ActionResult RoleBOnly()
        {
            return View();
        }
    }
}