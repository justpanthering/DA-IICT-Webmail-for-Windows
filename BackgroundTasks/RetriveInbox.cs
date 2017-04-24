using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    public sealed class RetriveInbox : IBackgroundTask
    {
        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
        CurrentCredentials1 currCred = new CurrentCredentials1();
        string json;
        RootObject1 newInboxMails = new RootObject1();
        RootObject1 InboxMails = new RootObject1();

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            GetCredentials();

            await GetInbox();

            deferral.Complete();
        }

        private async Task GetInbox()
        {
            InternetMachine1 machine = new InternetMachine1();
            json = await machine.login(currCred);
            if(json!=null && json!="")
            {
                newInboxMails = InboxJSONC1.serialize(json);
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFolder.GetFileAsync(currCred.Username+"_inboxjson");
                string oldjson = await FileIO.ReadTextAsync(file);
                InboxMails = InboxJSONC1.serialize(oldjson);

                findNewMails();

                await FileIO.WriteTextAsync(file, json);
            }
        }

        private void findNewMails()
        {
            int count = 0;
            string from = "";
            string t;

            while(count!=newInboxMails.m.Count-1)
            {
                if(!InboxMails.m.Any(mail => mail.cid == newInboxMails.m[count].cid))
                {
                    //push
                    var notificationXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText04);
                    var toastElement = notificationXml.GetElementsByTagName("text");
                    //toastElement[0].AppendChild(notificationXml.CreateTextNode(currCred.Username));
                    if(newInboxMails.m[count].su!="")
                        toastElement[0].AppendChild(notificationXml.CreateTextNode(newInboxMails.m[count].su));
                    else
                        toastElement[0].AppendChild(notificationXml.CreateTextNode("(No Subject)"));
                    foreach (E1 recipients in newInboxMails.m[count].e)
                    {
                        t = recipients.t;
                        if (t == "f")
                            from = recipients.a;
                    }
                    toastElement[1].AppendChild(notificationXml.CreateTextNode(from));
                    var launchAttribute = notificationXml.CreateAttribute("launch");
                    var toastNotification = new ToastNotification(notificationXml);
                    //toastNotification.Activated += Toast_Activated;
                    ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
                }
                count++;
            }
        }

        private void GetCredentials()
        {
            if (AppSettings.Values.ContainsKey("CurrUsername"))
            {
                currCred.Username = (string)AppSettings.Values["CurrUsername"];
                currCred.Password = (string)AppSettings.Values["CurrPassword"];
            }
        }
    }
}
