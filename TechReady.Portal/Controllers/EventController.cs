using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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
    public class EventController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: Event/Create
        //parameters: id of the existing event
        public ActionResult CreateEvent(int? id) 
        {
            if (id != null)
            {
                ViewBag.EventID = id;       //existing event.
            }
            else
            {
                ViewBag.EventID = 0;        //new event
            }
            return View();
        }


        // POST: Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //create new and modify existing event details.
        //parameters: event details object having event, event tracks and event techno log tags objects.
        [System.Web.Mvc.HttpPost]
        //[ValidateAntiForgeryToken]
        public String CreateEvent(EventInfoModel eventInfo)
        {
            if (eventInfo.Event.EventTypeObj != null)
            {
                eventInfo.Event.EventType = (EventType) eventInfo.Event.EventTypeObj.Key; // map key to enum type.
            }

            if (eventInfo.Event.EventStatusObj != null)
            {
                eventInfo.Event.EventStatus = (EventStatus)eventInfo.Event.EventStatusObj.Key; // map key to enum type.
            }


            if (eventInfo.Event.EventVisibilityObj    != null)
            {
                eventInfo.Event.EventVisibility = (EventVisibility)eventInfo.Event.EventVisibilityObj.Key; // map key to from enum type.
            }


            if (eventInfo.Event.EventScEligibilityObj != null)
            {
                eventInfo.Event.ScEligibility = (scEligibility)eventInfo.Event.EventScEligibilityObj.Key; // map key to enum type.
            }

            if (ModelState.IsValid)
            {
                if (eventInfo.Event.EventID == 0)                   //if new event.
                {
                    db.Events.Add(eventInfo.Event);                 //add new event.
                    if(eventInfo.EventTracks==null && (eventInfo.EventTechnologTags == null || eventInfo.EventAudienceTypeTags ==null)) //event technolog tags does not exist and event tracks are also empty.
                    {
                        return JsonConvert.SerializeObject(new { responseCode = 003 });
                    }
                    db.SaveChanges();
                    if (eventInfo.EventTracks != null)              //event tracks if exists.
                    {
                        foreach (var eventTrack in eventInfo.EventTracks)
                        {
                            eventTrack.EventID = eventInfo.Event.EventID;

                            db.EventTracks.Add(eventTrack);
                        }
                        if (eventInfo.EventTechnologTags != null)       //event log tags in event if exist.
                        {
                            var @event = db.Events.Where(x => x.EventID == eventInfo.Event.EventID).FirstOrDefault();
                            foreach (var eventLog in eventInfo.EventTechnologTags)
                            {
                                @event.EventTechnologTags.Remove(eventLog);
                            }
                        }
                        if (eventInfo.EventAudienceTypeTags != null)
                        {
                            var @event = db.Events.Where(x => x.EventID == eventInfo.Event.EventID).FirstOrDefault();
                            foreach (var eventLog in eventInfo.EventAudienceTypeTags)
                            {
                                @event.EventAudienceTypeTags.Remove(eventLog);
                            }
                        }
                    }
                    else
                    {
                        var @event = db.Events.Where(x => x.EventID == eventInfo.Event.EventID).FirstOrDefault();
                        @event.EventTechnologTags = new List<PrimaryTechnology>();
                        foreach (var eventLog in eventInfo.EventTechnologTags)          //event techno log tags exist.
                        {
                            var existingEventLog = db.PrimaryTechnologies.Where(x => x.PrimaryTechnologyID == eventLog.PrimaryTechnologyID).FirstOrDefault();
                            @event.EventTechnologTags.Add(existingEventLog);        //add to event.
                        }

                        @event.EventAudienceTypeTags = new List<AudienceType>();
                        foreach (var eventLog in eventInfo.EventAudienceTypeTags)          //event techno log tags exist.
                        {
                            var existingEventLog = db.AudienceTypes.Where(x => x.AudienceTypeID == eventLog.AudienceTypeID).FirstOrDefault();
                            @event.EventAudienceTypeTags.Add(existingEventLog);        //add to event.
                        }


                    }
                }
                else
                {
                    var existingEvent = db.Events
                        .Where(p => p.EventID == eventInfo.Event.EventID)
                        .Include("EventTracks.TrackAgendas")
                        .SingleOrDefault();

                    if (existingEvent != null)       //event already exist.
                    {
                        db.Entry(existingEvent).CurrentValues.SetValues(eventInfo.Event);       //update existing event values.

                        // Delete children
                        foreach (var existingChild in existingEvent.EventTracks.ToList())
                        {
                            if (eventInfo.EventTracks==null || !eventInfo.EventTracks.Any(c => c.EventTrackID == existingChild.EventTrackID))
                            {
                                existingChild.TrackAgendas.ToList().ForEach(p => db.TrackAgendas.Remove(p));
                                db.EventTracks.Remove(existingChild);
                            }
                               
                        }


                        if (eventInfo.EventTracks != null)
                        {
                            // Update and Insert children
                            foreach (var childModel in eventInfo.EventTracks)
                            {
                                var existingChild = existingEvent.EventTracks
                                    .Where(c => c.EventTrackID == childModel.EventTrackID)
                                    .SingleOrDefault();

                                if (existingChild != null)
                                {
                                    // Update child
                                    
                                    db.Entry(existingChild).CurrentValues.SetValues(childModel);

                                    // Delete children
                                    foreach (var existingGrandChild in existingChild.TrackAgendas.ToList())
                                    {
                                        if (!existingChild.TrackAgendas.Any(c => c.TrackAgendaID == existingGrandChild.TrackAgendaID))
                                            db.TrackAgendas.Remove(existingGrandChild);
                                    }

                                    // Update and Insert children
                                    foreach (var grandChildModel in existingChild.TrackAgendas)
                                    {
                                        var existingGrandChild = existingChild.TrackAgendas
                                            .Where(c => c.TrackAgendaID == grandChildModel.TrackAgendaID)
                                            .SingleOrDefault();

                                        if (existingGrandChild != null)
                                        {
                                            // Update child
                                            
                                            db.Entry(existingGrandChild).CurrentValues.SetValues(grandChildModel);
                                        }
                                        else
                                        {
                                            // Insert child
                                            var newChild = new TrackAgenda()
                                            {
                                                EndTime = grandChildModel.EndTime,
                                                SpeakerID = grandChildModel.SpeakerID,
                                                EventTrackID = grandChildModel.EventTrackID,
                                                FavCount = grandChildModel.FavCount,
                                                QRCode = grandChildModel.QRCode,
                                                SessionID = grandChildModel.SessionID,
                                                StartTime = grandChildModel.StartTime
                                            };
                                            existingChild.TrackAgendas.Add(newChild);
                                        }
                                    }


                                }
                                else
                                {
                                    // Insert child
                                    var newChild = new EventTrack()
                                    {
                                        EventID = childModel.EventID,
                                        TrackEndTime = childModel.TrackEndTime,
                                        TrackID = childModel.TrackID,
                                        TrackOwner = childModel.TrackOwner,
                                        TrackSeating = childModel.TrackSeating,
                                        TrackVenue = childModel.TrackVenue,
                                        TrackStartTime = childModel.TrackStartTime,
                                        TrackAgendas = childModel.TrackAgendas
                                    };
                                    existingEvent.EventTracks.Add(newChild);
                                }
                            }
                            if (existingEvent.EventTechnologTags != null)
                            {
                                var existingTags = existingEvent.EventTechnologTags.ToList();
                                foreach (var eventLog in existingTags)
                                {
                                    existingEvent.EventTechnologTags.Remove(
                                        existingEvent.EventTechnologTags.FirstOrDefault(
                                            x => x.PrimaryTechnologyID == eventLog.PrimaryTechnologyID));
                                }
                            }
                            if (existingEvent.EventAudienceTypeTags != null)
                            {
                                var existingTags = existingEvent.EventAudienceTypeTags.ToList();
                                foreach (var eventLog in existingTags)
                                {
                                    existingEvent.EventAudienceTypeTags.Remove(
                                        existingEvent.EventAudienceTypeTags.FirstOrDefault(
                                            x => x.AudienceTypeID == eventLog.AudienceTypeID));
                                }
                            }
                        }
                        else
                        {
                            if (eventInfo.EventTechnologTags == null)
                            {
                                return JsonConvert.SerializeObject(new { responseCode = 003 });
                            }
                            else
                            {
                                    if (existingEvent.EventTechnologTags != null)
                                    {

                                    var existingTags = existingEvent.EventTechnologTags.ToList();
                                    foreach (var eventLog in existingTags)
                                        {
                                            existingEvent.EventTechnologTags.Remove(eventLog);
                                        }
                                    }
                                    existingEvent.EventTechnologTags = new List<PrimaryTechnology>();
                                foreach (var eventLog in eventInfo.EventTechnologTags)
                                {
                                    var existingEventLog = db.PrimaryTechnologies.Where(x => x.PrimaryTechnologyID == eventLog.PrimaryTechnologyID).FirstOrDefault();
                                    existingEvent.EventTechnologTags.Add(existingEventLog);         //add new techno log tags of event.
                                }
                            }

                            if (eventInfo.EventAudienceTypeTags == null)
                            {
                                return JsonConvert.SerializeObject(new { responseCode = 003 });
                            }
                            else
                            {
                                if (existingEvent.EventAudienceTypeTags != null)
                                {

                                    var existingTags = existingEvent.EventAudienceTypeTags.ToList();
                                    foreach (var eventLog in existingTags)
                                    {
                                        existingEvent.EventAudienceTypeTags.Remove(eventLog);
                                    }
                                }
                                existingEvent.EventAudienceTypeTags = new List<AudienceType>();
                                foreach (var eventLog in eventInfo.EventAudienceTypeTags)
                                {
                                    var existingEventLog = db.AudienceTypes.Where(x => x.AudienceTypeID == eventLog.AudienceTypeID).FirstOrDefault();
                                    existingEvent.EventAudienceTypeTags.Add(existingEventLog);         //add new techno log tags of event.
                                }
                            }

                        }

                    }
                }
                db.SaveChanges();

                return JsonConvert.SerializeObject(new {responseCode = 001 });
            }
            return JsonConvert.SerializeObject(new { responseCode = 002 });
        }

        //get details of existing event.
        //parameters: existing event id.
        //GET: Event/GetEvent?id
        public string GetEvent(int id)
        {
            List<Object> eventDetails = new List<object>();
            Event @event = db.Events.Find(id);
            @event.EventTypeObj = new KeyValueObject()      //key-value mapping.
            {
                Key = (int) @event.EventType,
                Value = @event.EventType.ToString()
            };

            @event.EventStatusObj = new KeyValueObject()     //key-value mapping.
            {
                Key = (int)@event.EventStatus,
                Value = @event.EventStatus.ToString()
            };

            @event.EventVisibilityObj = new KeyValueObject()   //key-value mapping.
            {
                Key = (int)@event.EventVisibility,
                Value = @event.EventVisibility.ToString()
            };

            @event.EventScEligibilityObj = new KeyValueObject()     //key-value mapping.    
            {
                Key = (int)@event.ScEligibility,
                Value = @event.ScEligibility.ToString()
            };



            if (@event != null)
            {
                //get tracks of existing event.
                var tracksOfThisEvent = (from et in db.EventTracks where et.EventID==id
                                        join t in db.Tracks on et.TrackID equals t.TrackID
                                         select new { TrackDisplayName = t.TrackDisplayName,et.TrackVenue,et.TrackStartTime,et.TrackEndTime,et.TrackSeating,et.TrackOwner, et.EventTrackID,et.TrackID ,et.EventID}).ToList();

                eventDetails.Add(@event);
                eventDetails.Add(tracksOfThisEvent);
                return JsonConvert.SerializeObject(eventDetails,GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);
            }
            return JsonConvert.SerializeObject(eventDetails, GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);

        }

        [System.Web.Mvc.HttpPost]
        //adds/modifies track agendas of event track. 
        //POST: ModifyTrackAgendas
        public string ModifyTrackAgendas(List<TrackAgenda> trackAgendas)
        {

            var eventTrackID = Convert.ToInt32(trackAgendas[0].EventTrackID);           //id of event track.
            try {
                foreach (var trackAgenda in trackAgendas)
                {
                    var existingTrackAgenda = db.TrackAgendas
                       .SingleOrDefault(p => p.TrackAgendaID == trackAgenda.TrackAgendaID);

                    if (existingTrackAgenda!=null)
                    {//modify track agendas if exist.
                        db.Entry(existingTrackAgenda).CurrentValues.SetValues(trackAgenda);
                    }
                    else
                    {//add new track agendas.
                        db.TrackAgendas.Add(trackAgenda);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Track Agenda save error: " + e);
                return JsonConvert.SerializeObject(new { responseCode = 002 });
            }
            //sessions saved currently.
            var sessionsOfThisTrack = (from ta in db.TrackAgendas
                                       where ta.EventTrackID == eventTrackID
                                       join t in db.Sessions on ta.SessionID equals t.SessionID
                                       select new { Title = t.Title, ta.SpeakerID, ta.SessionID, ta.StartTime, ta.EndTime, ta.QRCode, ta.EventTrackID, ta.TrackAgendaID }).ToList();


            return JsonConvert.SerializeObject(new {responseCode = 001, trackAgendasSaved = sessionsOfThisTrack });
        }
    }
}