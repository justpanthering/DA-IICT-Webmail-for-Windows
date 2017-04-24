using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundTasks
{
    class InternetMachine1
    {
        string inbox = "https://webmail.daiict.ac.in/home/~/inbox.json";

        public async Task<string> login(CurrentCredentials1 cred)
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
            catch (Exception)
            {
                return null;
            }
        }
    }
}
