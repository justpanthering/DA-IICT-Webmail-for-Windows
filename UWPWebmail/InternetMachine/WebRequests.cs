using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace UWPWebmail.InternetMachine
{
    class WebRequests
    {
        string loginURI = "https://webmail.daiict.ac.in/service/home/~/inbox.rss?limit=1";
        string inbox = "https://webmail.daiict.ac.in/home/~/inbox.json";
        string sent_uri = "https://webmail.daiict.ac.in/home/~/sent.json";
        string trash_uri = "https://webmail.daiict.ac.in/home/~/trash.json";
        string webmail = "https://webmail.daiict.ac.in/home/~/?id=";

        public async Task<string> login(CurrentCredentials cred)
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            var byteArray = Encoding.ASCII.GetBytes(cred.Username + ":" + cred.Password);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            try
            { 
                var response = await client.GetStringAsync(inbox);
                return response;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async Task<string> sent(CurrentCredentials cred)
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            var byteArray = Encoding.ASCII.GetBytes(cred.Username + ":" + cred.Password);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            try
            {
                var response = await client.GetStringAsync(sent_uri);
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> trash(CurrentCredentials cred)
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            var byteArray = Encoding.ASCII.GetBytes(cred.Username + ":" + cred.Password);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            try
            {
                var response = await client.GetStringAsync(trash_uri);
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Stream> GetMail(CurrentCredentials cred, string id)
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            var byteArray = Encoding.ASCII.GetBytes(cred.Username + ":" + cred.Password);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            try
            {
                var response = await client.GetStreamAsync(webmail+id);
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
