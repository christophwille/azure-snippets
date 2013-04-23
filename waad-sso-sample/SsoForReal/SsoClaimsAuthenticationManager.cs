using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.WindowsAzure.ActiveDirectory;
using Microsoft.WindowsAzure.ActiveDirectory.GraphHelper;

namespace SsoForReal
{
    public class SsoClaimsAuthenticationManager : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (incomingPrincipal != null && incomingPrincipal.Identity.IsAuthenticated)
            {
                string upn = incomingPrincipal.Identity.Name;

                var ds = GetAuthenticatedDirectoryDataService(incomingPrincipal);

                var user = ds.directoryObjects.OfType<User>().Where(it => (it.userPrincipalName == upn)).SingleOrDefault();
                ds.LoadProperty(user, "memberOf");
                List<Group> groups = user.memberOf.OfType<Group>().ToList();

                var roleClaims = groups.Select(g => new Claim(ClaimTypes.Role, g.displayName, null, "CLAIMISSUER_GRAPH"));
                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaims(roleClaims);
            }

            return incomingPrincipal;
        }

        private DirectoryDataService GetAuthenticatedDirectoryDataService(ClaimsPrincipal incomingPrincipal)
        {
            //get the tenantName
            string tenantName = incomingPrincipal.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            // retrieve the clientId and password values from the Web.config file
            string clientId = ConfigurationManager.AppSettings["ClientId"];
            string password = ConfigurationManager.AppSettings["Password"];

            // get a token using the helper
            AADJWTToken token = DirectoryDataServiceAuthorizationHelper.GetAuthorizationToken(tenantName, clientId, password);

            // initialize a graphService instance using the token acquired from previous step
            return new DirectoryDataService(tenantName, token);
        }
    }
}