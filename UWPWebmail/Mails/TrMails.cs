using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPWebmail.Mails
{
    class TrMails : INotifyPropertyChanged
    {
        public string _to;
        public string To
        {
            get
            {
                return _to;
            }
            set
            {
                _to = value;
                NotifyPropertyChanged("To");
            }
        }

        public string _from;
        public string From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
                NotifyPropertyChanged("From");
            }
        }

        public string _subject;
        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
                NotifyPropertyChanged("Subject");
            }
        }

        public string _msg;
        public string msg
        {
            get
            {
                return _msg;
            }
            set
            {
                _msg = value;
                NotifyPropertyChanged("msg");
            }
        }

        public string _id;
        public string id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                NotifyPropertyChanged("id");
            }
        }

        public string _dateTime;
        public string DateTime
        {
            get
            {
                return _dateTime;
            }
            set
            {
                _dateTime = value;
                NotifyPropertyChanged("DateTime");
            }
        }

        public string _attach;
        public string Attach
        {
            get
            {
                return _attach;
            }
            set
            {
                _attach = value;
                NotifyPropertyChanged("Attach");
            }
        }

        public TrMails(string to, string from, string subject, string message, string id, string dateTime, string attach)
        {
            _to = to;
            _from = from;
            _subject = subject;
            _msg = message;
            _id = id;
            _dateTime = dateTime;
            _attach = attach;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
