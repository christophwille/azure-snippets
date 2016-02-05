using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Diagnostics;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;

namespace CustomerWebApp
{
    public partial class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        //private static string authority = aadInstance + tenantId;
        private static string authority = aadInstance + "common"; // p205

        private static string partnerClientId = ConfigurationManager.AppSettings["partner:ClientId"];
        private static string partnerTenantId = ConfigurationManager.AppSettings["partner:TenantId"];

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseErrorPage(new ErrorPageOptions()
            {
                ShowCookies = true,
                ShowEnvironment = true,
                ShowQuery = true,
                ShowExceptionDetails = true,
                ShowHeaders = true,
                ShowSourceCode = true,
                SourceCodeLineCount = 10
            });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    PostLogoutRedirectUri = postLogoutRedirectUri,
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        RedirectToIdentityProvider = RedirectToIdentityProvider,
                        SecurityTokenValidated = SecurityTokenValidated
                    },
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuers = new List<string>() { $"https://sts.windows.net/{tenantId}/", $"https://sts.windows.net/{partnerTenantId}/" },
                        ValidAudiences = new List<string>() { clientId, partnerClientId }
                    },
                });
        }

        private static Task RedirectToIdentityProvider(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            Trace.Write("RedirectToIdentityProvider"); // p164

            string redirectUri = context.OwinContext.Authentication?.AuthenticationResponseChallenge?.Properties?.RedirectUri;
            // string appBaseUrl = context.Request.Scheme + "://" + context.Request.Host + context.Request.PathBase;

            if ("/PartnerLogin/Welcome" == redirectUri)
            {
                context.ProtocolMessage.ClientId = partnerClientId;
            }

            return Task.FromResult(0);
        }

        private static Task SecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> securityTokenValidatedNotification)
        {
            Trace.Write("SecurityTokenValidated"); // p165 
            return Task.FromResult(0);
        }
    }
}
