using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechReady.Common.Models;

namespace TechReady.ViewModels
{
    public class SessionDetailsPageViewModel:BaseViewModel
    {
        private TrackSession _session;
        public TrackSession Session
        {
            get
            {
                return _session;
            }
            set
            {
                if(_session != value)
                {
                    _session = value;
                    OnPropertyChanged("Session");
                    OnPropertyChanged("PrerequisitesAvailable");
                    OnPropertyChanged("AbstractAvailable");
                    OnPropertyChanged("PostrequisitesAvailable");
                }
            }
        }

       public bool PrerequisitesAvailable
        {
            get
            {
                return this.Session!=null ?!string.IsNullOrEmpty(Session.Prerequisites):false;
            }
        }


        public bool PostrequisitesAvailable
        {
            get
            {
                return this.Session != null ? !string.IsNullOrEmpty(Session.Posrequisites) : false;
            }
        }

       
        public bool AbstractAvailable
        {
            get
            {
                return this.Session != null ? !string.IsNullOrEmpty(Session.Abstract) : false;
            }
        }

    }
}
