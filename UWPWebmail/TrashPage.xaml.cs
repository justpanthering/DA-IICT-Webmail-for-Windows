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
    public sealed partial class TrashPage : Page
    {
        RootObject_t TrashMails = new RootObject_t();
        bool viewMailState = false;

        public TrashPage()
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
            TrashMails = (RootObject_t)e.Parameter;
            if (TrashMails.m != null)
                DisplayTrash();
        }

        private void DisplayTrash()
        {
            ObservableCollection<TrMails> TM = new ObservableCollection<TrMails>();
            string su;
            string fr;
            bool f;
            string to = "";
            string from = "";
            string t;
            string id;
            double unixTimeStamp;
            string DateAndTime;
            string attach_path;

            foreach (M_t subject in TrashMails.m)
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

                foreach (E_t recipients in subject.e)
                {
                    t = recipients.t;
                    if (t == "t")
                        to = "To: " + recipients.a;
                    if (t == "f")
                        from = "From: " + recipients.a;
                        
                }

                if (subject.f == "sa" || subject.f == "a")
                    attach_path = "\xE723";
                else
                    attach_path = "";

                TM.Add(new TrMails(to, from, su, fr, id, DateAndTime, attach_path));
            }

            MailList.ItemsSource = TM;
        }

        private void MailList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TrMails selectedMail = (sender as ListView).SelectedItem as TrMails;
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
