using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using Owin;
using System.Security.Claims;

namespace wsfedowindemo
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            string realm = ConfigurationManager.AppSettings["ida:Realm"];
            string meta = ConfigurationManager.AppSettings["ida:FederationMetadataLocation"];

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            var wsfedOptions = new WsFederationAuthenticationOptions
            {
                MetadataAddress = meta,
                Wtrealm = realm,
            };

            wsfedOptions.Notifications = new WsFederationAuthenticationNotifications()
            {
                SecurityTokenValidated = (context) =>
                {
                    AuthenticationTicket ticket = context.AuthenticationTicket;

                    if (ticket.Identity != null && ticket.Identity.IsAuthenticated)
                    {
                        AddGroupClaims(ticket);
                    }

                    return Task.FromResult(0);
                }
            };

            app.UseWsFederationAuthentication(wsfedOptions);
        }

        public const string ClaimsIssuerName = "MyApp";
        private void AddGroupClaims(AuthenticationTicket ticket)
        {
            string upn = ticket.Identity.Name;
            string tenantId = GraphOperations.GetTenantId(ticket.Identity);

            try
            {
                var graphOps = new GraphOperations();
                graphOps.Initialize(tenantId);

                var user = graphOps.GetUser(upn, expandGroups: true);
                List<Group> groups = user.MemberOf.OfType<Group>().Select(mo => (Group)mo).ToList();

                var roleClaims = groups.Select(g => new Claim(ClaimTypes.Role, g.DisplayName, null, ClaimsIssuerName));
                ticket.Identity.AddClaims(roleClaims);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }
    }
}