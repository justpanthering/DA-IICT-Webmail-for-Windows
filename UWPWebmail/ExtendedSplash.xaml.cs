using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPWebmail.InternetMachine;
using UWPWebmail.JSONClasses;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPWebmail
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtendedSplash : Page
    {
        string username;
        string password;
        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
        public ExtendedSplash()
        {
            this.InitializeComponent();
        }

        private void initialize()
        {
            splashProgressRing.IsActive = true;
            if (AppSettings.Values.ContainsKey("CurrUsername"))
            {
                username = (string)AppSettings.Values["CurrUsername"];
                password = (string)AppSettings.Values["CurrPassword"];
                Login();
            }
            else
            {
                splashProgressRing.IsActive = false;
                this.Frame.Navigate(typeof(LoginPage));
            }
        }

        private async void Login()
        {
                CurrentCredentials cred = new CurrentCredentials(username, password);
                //Login
                WebRequests login = new WebRequests();
                string response = await login.login(cred);

                if (response == null)
                {
                    splashProgressRing.IsActive = false;
                    this.Frame.Navigate(typeof(LoginPage));
                }
                else
                {
                    splashProgressRing.IsActive = false;
                    this.Frame.Navigate(typeof(MainPage),response);
                }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            initialize();
        }
    }
}
