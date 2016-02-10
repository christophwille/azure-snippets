AAD Partner Login
======================

Scenario: A partner sells a Web app x-times to customers, but wants to retain a login for her AAD domain to this customer Web site (support, you-name-it). In the simplest way possible.

The implementation is via an AAD application at the partner that holds all x redirect uri to the x client application installations. The respective client application accepts its tenant 
and its client id, plus the tenant and client id of the partner application for login.

(Page number "pXXX" refer to the "Modern Authentication with Azure Active Directory for Web Applications" book)

Single Sign On (SSO) with Azure Active Directory (AAD)
======================

An enhanced sign-on MVC application based off of the AAD GA walkthroughs, built for DotNetCologne 2013.

Please see waad-sso-sample\SsoForReal\ReadMe.txt.

Mobile Services Authentication with Azure Active Directory (AAD) via Auth0
======================

You need: a Mobile Service (with the standard Todo table generated, Insert and Read permissions set 
to Only Authenticated Users). Also, you need an Auth0 account, that is configured for an enterprise 
connection to Azure AD plus under Add-ons for Mobile Services.

Inspired by

* [Authenticate Azure Mobile Services Apps with Everything using Auth0](http://blog.auth0.com/2013/03/17/Authenticate-Azure-Mobile-Services-apps-with-Everything-using-Auth0/) 
* [Generating your own ZUMO auth token (Day 8)](http://www.thejoyofcode.com/Generating_your_own_ZUMO_auth_token_Day_8_.aspx)

Please see waad-auth0-zumo\waadauth0zumo\MainPage.xaml.cs for the code.
