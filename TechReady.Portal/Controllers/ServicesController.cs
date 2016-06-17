using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using TechReady.Portal.Models;
using TechReady.Portal.ViewModels;

namespace TechReady.Portal.Controllers
{
    [System.Web.Mvc.Authorize]

    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    public class ServicesController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        //get data for dropdown list for select fields of event page.
        public string getViewModelData()
        {
            db.Configuration.LazyLoadingEnabled = false;

            EventViewModels eventViewModel = new EventViewModels();
            eventViewModel.AvailableCities = new List<City>(db.Cities);

            eventViewModel.ScEligibility = getDictionaryObj("scEligibility");

            eventViewModel.EventStatus = getDictionaryObj("EventStatus");

            eventViewModel.EventVisibility = getDictionaryObj("EventVisibility");

            eventViewModel.EventType = getDictionaryObj("EventType");

            eventViewModel.Speakers = new List<Speaker>(db.Speakers);

            eventViewModel.EventTechnologTags = new List<PrimaryTechnology>(db.PrimaryTechnologies);

            eventViewModel.EventAudienceTypeTags = new List<AudienceType>(db.AudienceTypes);

            var query = from t in db.Tracks select new { t.TrackID, t.TrackDisplayName };
            eventViewModel.Tracks = new List<Object>(query);
            return JsonConvert.SerializeObject(eventViewModel, GlobalConfiguration.Configuration.Formatters.JsonFormatter
            .SerializerSettings);
        }

        //get sessions/track agendas of event tracks of event.
        //parameters: id of track used in session refrence, id of the event track. 
        public string getSessionValues(int? id, int? eventTrackId)
        {
            if (id == null)
            {
                return JsonConvert.SerializeObject(new { message = "Bad Request" });
            }
            List<Object> sessionValues = new List<object>();
            if (eventTrackId == null)
            {
                var sessionsOfThisTrack = db.Sessions.Where(s => s.TrackID == id);
                sessionValues.Add(sessionsOfThisTrack.ToList());
            }
            else
            {
                var sessionsOfThisTrack = (from ta in db.TrackAgendas
                                         where ta.EventTrackID == eventTrackId
                                         join t in db.Sessions on ta.SessionID equals t.SessionID
                                         select new { Title = t.Title, ta.SpeakerID, ta.SessionID, ta.StartTime, ta.EndTime, ta.QRCode, ta.EventTrackID, ta.TrackAgendaID }).ToList();
                if (sessionsOfThisTrack.Count == 0) {
                    var sessions = db.Sessions.Where(s => s.TrackID == id);
                    sessionValues.Add(sessions.ToList());
                } else
                {
                    sessionValues.Add(sessionsOfThisTrack.ToList());
                }
            }

            return JsonConvert.SerializeObject(sessionValues);
        }

        private List<KeyValueObject> getDictionaryObj(string enumType)
        {
            var newDict = new List<KeyValueObject>();
            switch (enumType)
            {
                case "scEligibility":
                    {
                        foreach (var v in Enum.GetValues(typeof(scEligibility)).Cast<scEligibility>().ToList())
                        {
                            newDict.Add(
                                new KeyValueObject()
                                {
                                    Key = (int) v,
                                    Value = v.ToString()
                                });
                        }
                        break;
                    }
                case "EventStatus":
                    {
                        foreach (var v in Enum.GetValues(typeof(EventStatus)).Cast<EventStatus>().ToList())
                        {
                            newDict.Add(
                                 new KeyValueObject()
                                 {
                                     Key = (int)v,
                                     Value = v.ToString()
                                 });
                        }
                        break;
                    }
                case "EventVisibility":
                    {
                        foreach (var v in Enum.GetValues(typeof(EventVisibility)).Cast<EventVisibility>().ToList())
                        {
                            newDict.Add(
                                new KeyValueObject()
                                {
                                    Key = (int)v,
                                    Value = v.ToString()
                                });
                        }
                        break;
                    }
                case "EventType":
                    {
                        foreach (var v in Enum.GetValues(typeof(EventType)).Cast<EventType>().ToList())
                        {
                            newDict.Add(
                                new KeyValueObject()
                                {
                                    Key = (int)v,
                                    Value = v.ToString()
                                });
                        }
                        break;
                    }
            }
            return newDict;
        }
    }


    public class KeyValueObject
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
