using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPWebmail.JSONClasses;
using UWPWebmail.Mails;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
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
    public sealed partial class InboxPage : Page
    {
        RootObject InboxMails = new RootObject();
        bool viewMailState = false;

        public InboxPage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if(viewMailState)
            {
                VisualStateManager.GoToState(this, "NarrowLayout_MailList", false);
                viewMailState = false;
                e.Handled = true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            InboxMails = (RootObject)e.Parameter;
            if(InboxMails.m!=null)
                DisplayInbox();
        }

        private void DisplayInbox()
        {
            ObservableCollection<InMails> IM = new ObservableCollection<InMails>();
            string su;
            string fr;
            bool f;
            string from = "";
            string t;
            string id;
            string attach_path;
            Brush brush;
            double unixTimeStamp;
            string DateAndTime;

            foreach (M subject in InboxMails.m)
            {
                if (subject.su == "")
                    su = "(No Subject)";
                else
                    su = subject.su;

                if (subject.fr == null)
                    fr = "(Cannot Display Content)";
                else
                    fr = subject.fr;

                if (subject.f == null)
                {
                    f = false;
                    brush = new SolidColorBrush(Colors.Black);
                    attach_path = "";
                }
                else if(subject.f=="a" || subject.f=="au")
                {
                    f = false;
                    brush = new SolidColorBrush(Colors.Black);
                    attach_path = "\xE723";
                }
                else
                {
                    f = true;
                    brush = new SolidColorBrush(Colors.Red);
                    attach_path = "";
                }

                id = subject.id;

                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

                unixTimeStamp = Convert.ToDouble(subject.d) / 1000;
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();

                DateAndTime = dtDateTime.ToString("dd/MM/yyyy HH:mm");
                string date = dtDateTime.ToString("dd/MM/yyyy");
                string today = DateTime.Now.ToString("dd/MM/yyyy");
                if (date == today)
                    DateAndTime = dtDateTime.ToString("HH:mm");
                else
                    DateAndTime = dtDateTime.ToString("dd/MM/yyyy");

                foreach (E recipients in subject.e)
                {
                    t = recipients.t;
                    if (t == "f")
                        from = recipients.a;
                }

                IM.Add(new InMails (from, su, fr, attach_path, f, id, brush, DateAndTime));
            }

            MailList.ItemsSource = IM;
        }

        private void MailList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            InMails selectedMail = (sender as ListView).SelectedItem as InMails;
            string id = selectedMail.id;
            if (Window.Current.Bounds.Width < 720)
            {
                VisualStateManager.GoToState(this, "NarrowLayout_ViewMail", false);
                viewMailState = true;
            }
            ViewMail.Navigate(typeof(Mail), id);
        }
    }
}
