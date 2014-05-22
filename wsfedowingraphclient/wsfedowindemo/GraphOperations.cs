using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace wsfedowindemo
{
    public class GraphOperations
    {
        private const string TenantIdClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";
        private const string LoginUrl = "https://login.windows.net/{0}";
        private const string GraphUrl = "https://graph.windows.net";
        private static readonly string ClientId = ConfigurationManager.AppSettings["ida:ClientID"];
        private static readonly string ClientSecret = ConfigurationManager.AppSettings["ida:Password"];

        public static string GetTenantId(ClaimsPrincipal p)
        {
            return p.FindFirst(TenantIdClaimType).Value;
        }

        public static string GetTenantId(ClaimsIdentity ci)
        {
            return ci.FindFirst(TenantIdClaimType).Value;
        }

        private GraphConnection _graphConnection;
        public void Initialize(string tenantId)
        {
            var authContext = new AuthenticationContext(String.Format(CultureInfo.InvariantCulture, 
                LoginUrl,
                tenantId));

            var credential = new ClientCredential(ClientId, ClientSecret);

            AuthenticationResult authenticationResult = authContext.AcquireToken(GraphUrl, credential);
            string token = authenticationResult.AccessToken;

            var settings = new GraphSettings()
            {
                ApiVersion = "1.21-preview"
            };

            Guid clientRequestId = Guid.NewGuid();
            _graphConnection = new GraphConnection(token, clientRequestId, settings);
        }

        public void Initialize()
        {
            string tenantId = GetTenantId(ClaimsPrincipal.Current);
            Initialize(tenantId);
        }

        public User GetUser(string userPrincipalName, bool expandGroups)
        {
            var expression = ExpressionHelper.CreateEqualsExpression(typeof(User),
                GraphProperty.UserPrincipalName,
                userPrincipalName);

            var filter = new FilterGenerator()
            {
                QueryFilter = expression
            };

            if (expandGroups)
                filter.ExpandProperty = LinkProperty.MemberOf;

            PagedResults<User> pagedResults = _graphConnection.List<User>(null, filter);

            if (pagedResults.Results.Count > 0)
            {
                return pagedResults.Results[0];
            }

            return null;
        }

        public User GetUser(string objectId)
        {
            return _graphConnection.Get<User>(objectId);
        }
    }
}