using MimeKit;
using NotificationsExtensions.Toasts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UWPWebmail.InternetMachine;
using UWPWebmail.JSONClasses;
using Windows.Data.Pdf;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Notifications;
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
    public sealed partial class Mail : Page
    {
        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;

        public Mail()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ProgBarLoad.Visibility = Visibility.Visible;
            string id = (string)e.Parameter;
            if (id != null)
            {
                string user = (string)AppSettings.Values["Username"];
                string pass = (string)AppSettings.Values["Password"];
                CurrentCredentials cred = new CurrentCredentials(user, pass);

                WebRequests wr = new WebRequests();
                Stream resp = await wr.GetMail(cred, id);
                parseMessage(resp);
            }
            ProgBarLoad.Visibility = Visibility.Collapsed;
        }

        private void parseMessage(Stream resp)
        {
            var attachments = new List<MimePart>();
            var multiparts = new List<Multipart>();
            var message = MimeMessage.Load(resp);
            Date.Text = " " + message.Date.LocalDateTime.ToString("ddd d MMM yyyy HH:mm");
            //Cc.Text = " " + message.Cc.ToString();
            Subject.Text = " " + message.Subject;
            From.Text = " " + message.From.ToString();
            To.Text = " " + message.To.ToString();
            if (message.HtmlBody != null)
            {
                WebMessage.NavigateToString(message.HtmlBody);
            }
            else
            {
                MailContent.Text = message.TextBody;
            }
            if (message.Attachments.Count() != 0)
            {
                using (var iter = new MimeIterator(message))
                {
                    // collect our list of attachments and their parent multiparts
                    while (iter.MoveNext())
                    {
                        var multipart = iter.Parent as Multipart;
                        var part = iter.Current as MimePart;

                        if (multipart != null && part != null && part.IsAttachment)
                        {
                            // keep track of each attachment's parent multipart
                            multiparts.Add(multipart);
                            attachments.Add(part);
                        }
                    }
                }
                // now remove each attachment from its parent multipart...
                for (int i = 0; i < attachments.Count; i++)
                {
                    multiparts[i].Remove(attachments[i]);
                }
                AttachmentList.ItemsSource = attachments;
            }
            else
            {
                AttachmentTextBlock.Visibility = Visibility.Collapsed;
                AttachmentList.Visibility = Visibility.Collapsed;
            }
        }

        private async void Attachments_Click(object sender, RoutedEventArgs e)
        {
            ProgBar.Visibility = Visibility.Visible;
            ProgBar.Value = 0;
            var attachment = (MimePart)(sender as HyperlinkButton).DataContext;
            var fileName = attachment.FileName;
            ProgBar.Value = 5;
            byte[] buf = new byte[1000];
            await Task.Run(() =>
            {
                using (var stream = new MemoryStream())
                {
                    attachment.ContentObject.DecodeTo(stream);
                    buf = stream.ToArray();
                }
            });

            Windows.Storage.StorageFile sampleFile = await DownloadsFolder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
            await Windows.Storage.FileIO.WriteBytesAsync(sampleFile, buf);
            ProgBar.Value = 10;
            ProgBar.Visibility = Visibility.Collapsed;

            await Launcher.LaunchFileAsync(sampleFile);

            var notificationXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            var toastElement = notificationXml.GetElementsByTagName("text");
            toastElement[0].AppendChild(notificationXml.CreateTextNode("Download Complete"));
            toastElement[1].AppendChild(notificationXml.CreateTextNode("Open Downloads folder to view file " + fileName));
            var launchAttribute = notificationXml.CreateAttribute("launch");
            launchAttribute.Value = sampleFile.Path.ToString();
            var toastNotification = new ToastNotification(notificationXml);
            //toastNotification.Activated += Toast_Activated;
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }
    }
}