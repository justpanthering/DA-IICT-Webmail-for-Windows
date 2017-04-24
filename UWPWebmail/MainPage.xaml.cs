using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using UWPWebmail.InternetMachine;
using UWPWebmail.JSONClasses;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPWebmail
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
        RootObject InboxMails = new RootObject();
        RootObject_s SentMails = new RootObject_s();
        RootObject_t TrashMails = new RootObject_t();
        string response;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProgBar.Visibility = Visibility.Visible;
            if (Sent.IsSelected)
            {
                Title.Text = "Sent";
                //BackButton.Visibility = Visibility.Visible;

                string username = (string)AppSettings.Values["Username"];
                string password = (string)AppSettings.Values["Password"];
                CurrentCredentials cred = new CurrentCredentials(username, password);
                
                WebRequests wr = new WebRequests();
                string response = await wr.sent(cred);

                if (response == null)
                {
                    var dialog = new Windows.UI.Popups.MessageDialog("Can't Connect to the internet. Please check your connection and try again later.");
                    await dialog.ShowAsync();
                    return;
                }

                SentMails = SentJSONC.serialize(response);
                ListMail.Navigate(typeof(SentPage), SentMails);
            }

            else if(Inbox.IsSelected)
            {
                Title.Text = "Inbox";
                string username = (string)AppSettings.Values["Username"];
                string password = (string)AppSettings.Values["Password"];
                CurrentCredentials cred = new CurrentCredentials(username, password);
                
                WebRequests wr = new WebRequests();
                string response = await wr.login(cred);

                if (response == null)
                {
                    var dialog = new Windows.UI.Popups.MessageDialog("Can't Connect to the internet. Please check your connection and try again later.");
                    await dialog.ShowAsync();
                    return;
                }

                InboxMails = InboxJSONC.serialize(response);

                //AppSettings.Values[cred.Username + "_inboxjson"] = response;
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFolder.CreateFileAsync(cred.Username+"_inboxjson", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, response);

                RegisterBackgroundTask();

                ListMail.Navigate(typeof(InboxPage), InboxMails);
            }

            else if(Logout.IsSelected)
            {
                this.Frame.Navigate(typeof(LoginPage));
            }

            else if(Trash.IsSelected)
            {
                Title.Text = "Trash";
                string username = (string)AppSettings.Values["Username"];
                string password = (string)AppSettings.Values["Password"];
                CurrentCredentials cred = new CurrentCredentials(username, password);

                WebRequests wr = new WebRequests();
                string response = await wr.trash(cred);

                if (response == null)
                {
                    var dialog = new Windows.UI.Popups.MessageDialog("Can't Connect to the internet. Please check your connection and try again later.");
                    await dialog.ShowAsync();
                    return;
                }

                TrashMails = TrashJSONC.serialize(response);
                ListMail.Navigate(typeof(TrashPage), TrashMails);
            }

            else if(Compose.IsSelected)
            {
                Title.Text = "Compose";
                var dialog = new MessageDialog("This feature is not available right now in this app. Do you want to open in browser?");
                dialog.Title = "Open in browser?";
                dialog.Commands.Add(new UICommand { Label = "Open in browser", Id = 0 });
                dialog.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
                var res = await dialog.ShowAsync();

                if ((int)res.Id == 0)
                {
                    await Windows.System.Launcher.LaunchUriAsync(new Uri("https://webmail.daiict.ac.in", UriKind.Absolute));
                }
            }
            if(HamburgerMenu.IsPaneOpen)
                HamburgerMenu.IsPaneOpen = !HamburgerMenu.IsPaneOpen;
            ProgBar.Visibility = Visibility.Collapsed;
        }

        private async void RegisterBackgroundTask()
        {
            var taskName = "InboxUpdate";

            var backgroundStatusAccess = await BackgroundExecutionManager.RequestAccessAsync();

            if(backgroundStatusAccess == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity || backgroundStatusAccess == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach(var task in BackgroundTaskRegistration.AllTasks)
                {
                    if(task.Value.Name == taskName)
                    {
                        return;
                    }
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = taskName;
                taskBuilder.TaskEntryPoint = typeof(BackgroundTasks.RetriveInbox).FullName;
                taskBuilder.SetTrigger(new TimeTrigger(15, false));

                var Registration = taskBuilder.Register();
            }
        }

        private void HamburgerButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            HamburgerMenu.IsPaneOpen = !HamburgerMenu.IsPaneOpen;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BackButton.Visibility = Visibility.Collapsed;
            //Inbox.IsSelected = true;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string username = (string)AppSettings.Values["Username"];
            string password = (string)AppSettings.Values["Password"];
            CurrentCredentials cred = new CurrentCredentials(username, password);

            string response = (string)e.Parameter;
            InboxMails = InboxJSONC.serialize(response);

            //AppSettings.Values[cred.Username + "_inboxjson"] = response;
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync(cred.Username + "_inboxjson", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, response);

            RegisterBackgroundTask();
            Title.Text = "Inbox";
            ListMail.Navigate(typeof(InboxPage), InboxMails);
        }
    }
}