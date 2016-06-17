using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using TechReady.Models;

namespace TechReady.Common.Models
{
    public class Event : INotifyPropertyChanged
    {
        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Event c = (Event)obj;
            return (EventId == c.EventId);
        }
        public override int GetHashCode()
        {
            return this.EventId.GetHashCode();
        }


        public int EventId { get; set; }

        public string CityName { get; set; }
        public DateTime? EventFromDate { get; set; }
        public DateTime? EventToDate { get; set; }

        public string EventAbstract { get; set; }

        public string EventName { get; set; }

        public string EventStatus { get; set; }

        public string EventType { get; set; }

        public string EventVenue { get; set; }

        public string EventRegLink { get; set; }

        public bool GlobalEvent { get; set; }

        public List<EventTrack> Tracks { get; set; }

        public string EventFromTo
        {
            get
            {
                if (this.EventFromDate.HasValue && this.EventToDate.HasValue)
                {
                    if (this.EventFromDate.Value.Date == this.EventToDate.Value.Date)
                    {
                        return this.EventFromDate.Value.ToString("ddd dd-MMM-yyyy");
                    }
                    else
                    {
                        return this.EventFromDate.Value.ToString("ddd dd-MMM-yyyy") + " To " +
                               this.EventToDate.Value.ToString("ddd dd-MMM-yyyy ");

                    }
                }
                else
                {
                    return "";
                }

            }

            
        }


        private List<string> _eventTechnologies;
        
        public List<string> EventTechnologies
        {
            get
            {
               
                return _eventTechnologies;
            }
            set
            {
                if (_eventTechnologies != value)
                {
                    _eventTechnologies = value;
                    OnPropertyChanged("EventTechnologies");
                }
            }
        }


        private List<string> _eventRoles;


        public List<string> EventRoles
        {
            get
            {

                return _eventRoles;

            }
            set
            {
                if (_eventRoles != value)
                {
                    _eventRoles = value;
                    OnPropertyChanged("EventRoles");
                }
            }
        }

        public string VenueCity
        {
            get
            {
                if (!string.IsNullOrEmpty(this.EventVenue) && !string.IsNullOrEmpty(this.CityName))
                {
                    return this.EventVenue + " - " +
                           this.CityName;
                }
                return "";

            }
        }


        public bool HasMultipleTracks
        {
            get
            {
                if (this.Tracks != null && this.Tracks.Count > 1)
                {
                    return true;
                }
                return false;
            }
        }

        public string TechnologiesAbstract
        {
            get
            {
                if (this.Tracks == null || this.Tracks.Count==0)
                {

                    var stringBuilder = new StringBuilder();
                    for (int i = 0; i < EventTechnologies.Count; i++)
                    {
                        stringBuilder.Append(EventTechnologies[i]);
                        if (i != EventTechnologies.Count - 1)
                        {
                            stringBuilder.Append(" | ");
                        }
                    }
                    return stringBuilder.ToString();
                }
                else
                {
                    var stringBuilder = new StringBuilder();
                    List<string> secondaryTechnologies = new List<string>();
                    foreach (var eventTrack in this.Tracks)
                    {
                        if (eventTrack.Sessions == null)
                        {
                            continue;
                        }
                        foreach (var trackSession in eventTrack.Sessions)
                        {
                            if (string.IsNullOrEmpty(trackSession.SecondaryTechnologyName))
                            {
                                continue;

                            }
                            if (secondaryTechnologies.Contains(trackSession.SecondaryTechnologyName) == false)
                            {
                                secondaryTechnologies.Add(trackSession.SecondaryTechnologyName);
                            }
                        }
                    }
                    for (int i = 0; i < secondaryTechnologies.Count; i++)
                    {
                        stringBuilder.Append(secondaryTechnologies[i]);
                        if (i != secondaryTechnologies.Count - 1)
                        {
                            stringBuilder.Append(" | ");
                        }
                    }
                    return stringBuilder.ToString();

                }
            
            }
        }


        private EventTrack _currentTrack;

        [JsonIgnore]
        public EventTrack CurrentTrack
        {
            get
            {
                if (_currentTrack == null && this.Tracks != null && this.Tracks.Count > 0)
                {
                    _currentTrack = this.Tracks[0];
                }
                return _currentTrack;
            }
            set
            {
                if (_currentTrack != value)
                {
                    _currentTrack = value;
                    OnPropertyChanged("CurrentTrack");
                }
            }
        }

    

        private bool _isFollowed;

        public bool IsFollowed
        {
            get { return _isFollowed; }
            set
            {
                if (_isFollowed != value)
                {
                    this._isFollowed = value;
                    OnPropertyChanged("IsFollowed");

                }
            }
        }

        public int Weightage   { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

   public class EventTrack
    {
     

       public int EventTrackId { get; set; }

        public int TrackId { get; set; }

        public DateTime? TrackStartTime { get; set; }
        public DateTime? TrackEndTime { get; set; }

        public string TrackVenue { get; set; }

        public string Format { get; set; }

        public int AudienceTypeId { get; set; }
        public string AudienceType { get; set; }

        public string TrackAbstract { get; set; }

        public string Theme { get;set; }

        public string TrackDisplayName { get; set; }

        public List<TrackSession> Sessions { get; set; }


        public string TrackFromTo
        {
            get
            {
                if (this.TrackStartTime.HasValue && this.TrackEndTime.HasValue)
                {

                    return this.TrackStartTime.Value.ToString("dd-MMM-yyyy hh:mm:ss") + " To " +
                           this.TrackEndTime.Value.ToString("dd-MMM-yyyy hh:mm:ss");

                }
                else
                {
                    return "";
                }

            }
        }

        [JsonIgnore]
        public List<TrackSessionsDatedView> TrackSessionsDated
        {
            get
            {
                var dates = (from c in this.Sessions
                             group c by c.SessionStartTime.Value.Date
                    into g
                             select new TrackSessionsDatedView()
                             {
                                 TrackDate = g.Key,
                                 TrackDateSessions = g.ToList()
                             }).ToList();

                return dates;
            }
        }

       public List<TrackSpeaker> TrackSpeakers
       {
           get
           {
               var speakers = new List<TrackSpeaker>();
               if (this.Sessions != null && this.Sessions.Count > 0)
               {
                   foreach (var trackSession in this.Sessions)
                   {
                       if (speakers.FirstOrDefault(x => x.SpeakerName == trackSession.Speaker.SpeakerName) == null)
                       {
                           speakers.Add(trackSession.Speaker);
                       }
                   }
               }
               return speakers;
           }
       }


        public override string ToString()
        {
            return this.TrackDisplayName;
        }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            EventTrack c = (EventTrack)obj;
            return (EventTrackId == c.EventTrackId);
        }
        public override int GetHashCode()
        {
            return this.EventTrackId.GetHashCode();
        }
    }

    public class TrackSession
    {
        public int TrackAgendaId { get; set; }

        public TrackSpeaker Speaker { get; set; }

     

        public DateTime? SessionStartTime { get; set; }

        public DateTime? SessionEndTime { get; set; }

        public string SessionFromTo
        {
            get
            {
                if (this.SessionStartTime.HasValue && this.SessionEndTime.HasValue)
                {

                    return this.SessionStartTime.Value.ToString("HH:mm") + " To " +
                           this.SessionEndTime.Value.ToString("HH:mm");

                }
                else
                {
                    return "";
                }

            }
        }

        


        public int SessionNo { get; set; }
        public string Title { get; set; }

        public string Abstract { get; set; }

        public int TechLevel { get; set; }

        public int PrimaryTechnologyId { get; set; }

        public string PrimaryTechnologyName { get; set; }

        public int SecondaryTechnologyId { get; set; }

        public string SecondaryTechnologyName { get; set ; }

        public string Product { get; set; }

        public string Prerequisites { get; set; }

        public string Posrequisites { get; set; }
        
    }

    public class TrackSpeaker : INotifyPropertyChanged
    {
        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            TrackSpeaker c = (TrackSpeaker)obj;
            return (SpeakerId == c.SpeakerId);
        }
        public int SpeakerId { get; set; }
        public string SpeakerName { get; set; }

        public string Abstract { get; set; }

        public string Profile { get; set; }

        public string PicUrl { get; set; }

        public string Affiliation { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string SpeakerType { get; set; }
        
        public string Location { get; set; }

        public string TwitterLink { get; set; }

        public string FacebookLink { get; set; }
        public string LinkedinLink { get; set; }

        public string BlogLink { get; set; }

        private bool _isFollowed;


        public event Action FollowedChanged;

        public bool IsFollowed
        {
            get { return _isFollowed; }
            set
            {
                    this._isFollowed = value;
                    if (FollowedChanged != null)
                    {
                        this.FollowedChanged();
                    }
                    OnPropertyChanged("IsFollowed");
            }
        }

        private ObservableCollection<Event> _speakerEvents;

        [JsonIgnore]
        public ObservableCollection<Event> SpeakerEvents
        {
            get
            {
                return _speakerEvents;
            }
            set
            {
                if (_speakerEvents != value)
                {
                    _speakerEvents = value;
                    OnPropertyChanged("SpeakerEvents");
                }
            }
        }

        private ObservableCollection<LearningResource> _blogs;
        public ObservableCollection<LearningResource> Blogs
        {
            get
            {
                return _blogs;
            }
            set
            {

                _blogs = value;
                OnPropertyChanged("Blogs");

            }
        }

        public void NotifyPropertyChanges()
        {
            OnPropertyChanged("Blogs");
            OnPropertyChanged("SpeakerEvents");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }


    public class LoadMore : Event
    {
        
    }

    public class TrackSessionsDatedView
    {
        public DateTime? TrackDate;
        public List<TrackSession> TrackDateSessions { get; set; }

        public string TrackDateString
        {
            get
            {
                if (TrackDate.HasValue)
                {
                    return TrackDate.Value.ToString("dd-MM-yyyy");
                }
                return "";
            }
        }
    }
}
