# AAD Partner Scenario

A partner has access to 1..n customer application deployments. Instead of 
a multi-tenant approach (which would mean service principals back in the partner after consent), this approach
uses valid issuer / valid audience lists.

This scenario has an intended restriction: to not use authorization tokens from the users. All if any
permissions to other applications are application permissions.

## Login Flow

For the customer, the login flow is as usual

For the partner, the endpoint to sign in is always /PartnerLogin/SignIn (note that SignOut is atm not 
implemented)

## The "Magic"

The customer applications needs to know the tenant id of the partner, and a client id of an AAD application
from that partner. This AAD application holds the list of all valid customer application redirect uris (can
be easily maintained via Directory Graph API).

![multiple reply urls defined](multireplyurlsonpartnerapp.png?raw=true")

The aforementioned partner Web application is included, all it does is list customer A's login endpoint
on the /home/index page after login (strictly speaking it wouldn't even need to be online).

For code please see in customer solution [Startup.Auth.cs](https://github.com/christophwille/azure-snippets/blob/master/aad-partnerscenario/CustomerWebApp/App_Start/Startup.Auth.cs) and [PartnerLoginController.cs](https://github.com/christophwille/azure-snippets/blob/master/aad-partnerscenario/CustomerWebApp/Controllers/PartnerLoginController.cs), in partner solution [Startup.Auth.cs](https://github.com/christophwille/azure-snippets/blob/master/aad-partnerscenario/PartnerWebApp/App_Start/Startup.Auth.cs) is mildly interesting.
