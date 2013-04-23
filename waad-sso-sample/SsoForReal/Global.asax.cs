using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SsoForReal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //
            // http://blog.rytmis.net/2012/12/windows-azure-active-directory_10.html
            //
            // AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Upn;

            // RefreshValidationSettings();
        }

        // 
        // http://www.cloudidentity.com/blog/2013/04/02/auto-update-of-the-signing-keys-via-metadata/
        //
        protected void RefreshValidationSettings()
        {
            string configPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + "Web.config";
            string metadataAddress = ConfigurationManager.AppSettings["ida:FederationMetadataLocation"];
            ValidatingIssuerNameRegistry.WriteToConfig(metadataAddress, configPath);
        }
    }
}