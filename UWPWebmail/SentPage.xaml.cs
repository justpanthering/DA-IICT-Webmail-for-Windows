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
    public sealed partial class SentPage : Page
    {
        RootObject_s SentMails = new RootObject_s();
        bool viewMailState = false;

        public SentPage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (viewMailState)
            {
                VisualStateManager.GoToState(this, "NarrowLayout_MailList", false);
                viewMailState = false;
                e.Handled = true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SentMails = (RootObject_s)e.Parameter;
            if(SentMails.m!=null)
                DisplayInbox();
        }

        private void DisplayInbox()
        {
            ObservableCollection<SeMail> IM = new ObservableCollection<SeMail>();
            string su;
            string fr;
            bool f;
            string to = "";
            string t;
            string id;
            double unixTimeStamp;
            string DateAndTime;
            string attach_path;

            foreach (M_s subject in SentMails.m)
            {
                if (subject.su == "")
                    su = "(No Subject)";
                else
                    su = subject.su;

                if (subject.fr == null)
                    fr = "(Cannot Display Content)";
                else
                    fr = subject.fr;

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

                foreach (E_s recipients in subject.e)
                {
                    t = recipients.t;
                    if (t == "t")
                        to = recipients.a;
                }

                if (subject.f == "sa")
                    attach_path = "\xE723";
                else
                    attach_path = "";

                IM.Add(new SeMail(to, su, fr, id, DateAndTime, attach_path));
            }

            MailList.ItemsSource = IM;
        }

        private void MailList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SeMail selectedMail = (sender as ListView).SelectedItem as SeMail;
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
