# AAD Partner Scenario

A partner has access to 1..n customer application deployments. Instead of 
a multi-tenant approach (which would incur service principals back in partner application), this approach
uses valid issuer / valid audience lists.

## Login Flow

For the customer, the login flow is as usual

For the partner, the endpoint to sign in is always /PartnerLogin/SignIn (not that SignOut is atm not 
implemented)

## The "Magic"

The customer applications needs to know the tenant id of the partner, and a client id of an AAD application.
This AAD application holds the list of all valid customer application redirect uris.

![multiple reply urls defined](multireplyurlsonpartnerapp.png?raw=true")

For code please see in customer solution [Startup.Auth.cs](https://github.com/christophwille/azure-snippets/blob/master/aad-partnerscenario/CustomerWebApp/App_Start/Startup.Auth.cs) and [PartnerLoginController.cs](https://github.com/christophwille/azure-snippets/blob/master/aad-partnerscenario/CustomerWebApp/Controllers/PartnerLoginController.cs), in partner solution [Startup.Auth.cs](https://github.com/christophwille/azure-snippets/blob/master/aad-partnerscenario/PartnerWebApp/App_Start/Startup.Auth.cs) is mildly interesting.
