using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TechReady.Common.Models;

namespace TechReady.Models
{
    public class LearningResource : INotifyPropertyChanged
    {
        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            LearningResource c = (LearningResource)obj;
            return (LearningResourceID == c.LearningResourceID);
        }
        public override int GetHashCode()
        {
            return this.LearningResourceID.GetHashCode();
        }

        public int LearningResourceID { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string LearningResourceType { get; set; }
        public string PrimaryTechnologyName { get; set; }
        public DateTime PublicationTime { get; set; }
        public DateTime LastWatchedTime { get; set; }
        public string Thumbnail { get; set; }

        public List<AudienceType> AudienceTypes { get; set; }
        public int Weitage { get; set; }


        private bool _favourited;

        public bool Favourited
        {
            get { return _favourited; }
            set
            {
                if (_favourited != value)
                {
                    _favourited = value;
                    OnPropertyChanged("Favourited");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class LoadMoreLearningResource : LearningResource
    {

    }
}
