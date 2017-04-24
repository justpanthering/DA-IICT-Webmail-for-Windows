using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPWebmail.JSONClasses
{
    class MailFormat
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public string msg { get; set; }
        public DateTime date_time { get; set; }
        public string HtmlBody { get; set; }
    }
}
