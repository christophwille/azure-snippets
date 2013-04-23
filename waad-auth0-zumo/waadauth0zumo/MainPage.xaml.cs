using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;


namespace waadauth0zumo
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        // Note: There is no UI for inserting / showing data, all in code below!
        //
        // You need an Auth0 account, with either an Office 365 or a WAAD connection
        //
        // In addition, you have to add your key for Zumo (Mobile Services) in Auth0
        // http://blog.auth0.com/2013/03/17/Authenticate-Azure-Mobile-Services-apps-with-Everything-using-Auth0/
        //
        // Mobile Services: the default table should be generated (dashboard)
        //
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            const string zumoUrl = "https://waadthefsck.azure-mobile.net/";
            const string zumoApplicationKey = "REMOVED";

            const string auth0Home = "christophwille";
            const string auth0ClientId = "REMOVED";
            const string waadConnection = "waadthefsck.onmicrosoft.com";

            string startUri = String.Format(
                "https://{0}.auth0.com/authorize/?client_id={1}&redirect_uri=http://localhost:8181/callback&response_type=token&connection={2}&scope=openid",
                auth0Home,
                auth0ClientId,
                waadConnection);

            string endUri = "http://localhost:8181/callback";

            // Note: Set a breakpoint here to step through the sample
            var result = 
                await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, new Uri(startUri), new Uri(endUri));

            var respUri = new Uri(result.ResponseData);
            var decoder = new WwwFormUrlDecoder(respUri.Fragment.Substring(1)); // remove the leading #, because it is the fragment and not the query 
            string jwttoken = decoder.GetFirstValueByName("id_token");

            var mobileService = new MobileServiceClient(
                            zumoUrl,
                            zumoApplicationKey
                        );

            // Make sure that Insert and Read permissions are set to "Only Authenticated Users"
            var user = new MobileServiceUser("waaddidyouexpect")
            {
                MobileServiceAuthenticationToken = jwttoken
            };
            mobileService.CurrentUser = user;

            var todoTable = mobileService.GetTable<TodoItem>();

            await todoTable.InsertAsync(new TodoItem()
                                    {
                                        Text = "With greetings from a WAAD user " + DateTime.Now.Ticks,
                                        Complete = false
                                    });

            var results = await todoTable
                                    .Where(todoItem => todoItem.Complete == false)
                                    .ToListAsync();
        }
    }

    public class TodoItem
    {
        public int Id { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "complete")]
        public bool Complete { get; set; }
    }
}
