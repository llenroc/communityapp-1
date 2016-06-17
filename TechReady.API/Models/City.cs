using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.API.Models;
using System.ComponentModel;

namespace TechReady.Common.Models
{
    public class City
    {
        public string CityName { get; set; }
        public GeoCodeItem Location { get; set; }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            City c = (City)obj;
            return (CityName == c.CityName);
        }
        public override int GetHashCode()
        {
            return this.CityName.GetHashCode();
        }

        public override string ToString()
        {
            return CityName;
        }
    }


    public class Notification : System.ComponentModel.INotifyPropertyChanged
    {
        private int _notificationID;
        public int NotificationID { get
            {
                return _notificationID;
            }
            set
            {
                if(_notificationID != value)
                {
                    _notificationID = value;
                    OnPropertyChanged("NotificationID");
                }
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged("Message");
                }
            }
        }

        private string _actionLink;
        public string ActionLink
        {
            get
            {
                return _actionLink;
            }
            set
            {
                if (_actionLink != value)
                {
                    _actionLink = value;
                    OnPropertyChanged("ActionLink");
                }
            }
        }

        private int? _resourceId;
        public int? ResourceId
        {
            get
            {
                return _resourceId;
            }
            set
            {
                if (_resourceId != value)
                {
                    _resourceId = value;
                    OnPropertyChanged("ResourceId");
                }
            }
        }

        private DateTime _pushDateTime;
        public DateTime PushDateTime
        {
            get
            {
                return _pushDateTime;
            }
            set
            {
                if (_pushDateTime != value)
                {
                    _pushDateTime = value;
                    OnPropertyChanged("PushDateTime");
                }
            }
        }

        private bool _read;
        public bool Read
        {
            get
            {
                return _read;
            }
            set
            {
                if (_read != value)
                {
                    _read = value;
                    OnPropertyChanged("Read");
                }
            }
        }

        private bool _removed;
        public bool Removed
        {
            get
            {
                return _removed;
            }
            set
            {
                if (_removed != value)
                {
                    _removed = value;
                    OnPropertyChanged("Removed");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private int _notificationType;

        public int NotificationType
        {
            get
            {
                return _notificationType;
            }
            set
            {
                if (_notificationType != value)
                {
                    _notificationType = value;
                    OnPropertyChanged("NotificationType");
                }
            }
        }
    }
}
