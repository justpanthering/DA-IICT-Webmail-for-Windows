using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPWebmail.InternetMachine;
using UWPWebmail.JSONClasses;
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
    public sealed partial class LoginPage : Page
    {
        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void LogIn_Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (UsernameTextBox.Text == "" || PasswordTextBox.Password == "")
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Please enter Username/Password");
                await dialog.ShowAsync();
                return;
            }

            CurrentCredentials cred = new CurrentCredentials(UsernameTextBox.Text, PasswordTextBox.Password);

            if (RememberMe.IsChecked.Equals(true))
            {
                //Save Username and Password
                AppSettings.Values["Username"] = UsernameTextBox.Text;
                AppSettings.Values["Password"] = PasswordTextBox.Password;
            }

            AppSettings.Values["CurrUsername"] = UsernameTextBox.Text;
            AppSettings.Values["CurrPassword"] = PasswordTextBox.Password;

            ProgRing.IsActive = true;

            //Login
            WebRequests login = new WebRequests();
            string response = await login.login(cred);

            if (response == null)
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Unable to Login. \nPlease check Username/Password and try again.");
                await dialog.ShowAsync();
                ProgRing.IsActive = false;
                return;
            }

            RootObject Inbox = InboxJSONC.serialize(response);
            ProgRing.IsActive = false;
            this.Frame.Navigate(typeof(MainPage),response);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppSettings.Values.ContainsKey("Username"))
            {
                UsernameTextBox.Text = (string)AppSettings.Values["Username"];
                PasswordTextBox.Password = (string)AppSettings.Values["Password"];
            }
        }
    }
}