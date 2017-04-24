using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace UWPWebmail.Mails
{
    class InMails : INotifyPropertyChanged
    {
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

        public bool _IsUnread;
        public bool IsUnread
        {
            get
            {
                return _IsUnread;
            }
            set
            {
                _IsUnread = value;
                NotifyPropertyChanged("IsUnread");
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

        public Brush _brush;
        public Brush brush
        {
            get
            {
                return _brush;
            }
            set
            {
                _brush = value;
                NotifyPropertyChanged("brush");
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

        public InMails(string from, string subject, string message, string attach, bool isunread, string id, Brush brush, string dateTime)
        {
            _from = from;
            _subject = subject;
            _msg = message;
            _IsUnread = isunread;
            _id = id;
            _brush = brush;
            _dateTime = dateTime;
            _attach = attach;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
