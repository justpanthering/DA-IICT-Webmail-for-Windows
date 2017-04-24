using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPWebmail.InternetMachine
{
    class CurrentCredentials
    {
        public string Username;
        public string Password;
        public CurrentCredentials(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
    }
}
