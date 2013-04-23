using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace SsoForReal
{
    //
    // http://leastprivilege.com/2012/04/04/identity-in-net-4-5part-2-claims-transformation-in-asp-net-beta-1/
    //
    public class ClaimsTransformationHttpModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += Context_PostAuthenticateRequest;
        }

        void Context_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;

            // no need to call transformation if session already exists
            if (FederatedAuthentication.SessionAuthenticationModule != null && FederatedAuthentication.SessionAuthenticationModule.ContainsSessionTokenCookie(context.Request.Cookies))
            {
                return;
            }

            var transformer = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.ClaimsAuthenticationManager;

            if (transformer != null)
            {
                var transformedPrincipal = transformer.Authenticate(context.Request.RawUrl, context.User as ClaimsPrincipal);

                context.User = transformedPrincipal;
                Thread.CurrentPrincipal = transformedPrincipal;
            }
        }
    }
}