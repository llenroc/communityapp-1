using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Resources;
using System.Web.Mvc;
using TechReady.Portal.Models;

namespace TechReady.Portal.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
    [Authorize]
    public class EventsController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: Events
        public ActionResult Index()
        {
            //var events = db.Events.Include(@ => @.City);
            //return View(events.ToList());

            return View(db.Events.ToList());
        }

        public ActionResult CurrentEvents()
        {
            //var events = db.Events.Include(@ => @.City);
            //return View(events.ToList());
            var events =
                db.Events.ToList();

            return View(events.Where(
                    x => !(x.EventToDate.HasValue && (DateTime.Now.Date - x.EventToDate.Value.Date).TotalDays > 1)).ToList());
        }


        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CityName");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,EventName,EventAbstract,EventFromDate,EventToDate,EventVenue,RegLink,MaxCapacity,ScEligibility,RegCapacity,PubtoMSCOM,PostRegistered,PostAttended,PostManualOverallRating,EventOwner,EventVisibility,EventType,CityName")] Event @event)
        {
            if (ModelState.IsValid)
            {
                //@event.EventStatus = 1 ;  //See how to assign to Enum

                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityName = new SelectList(db.Cities, "CityName", "State", @event.CityName);
            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityName = new SelectList(db.Cities, "CityName", "CityName", @event.CityName);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventID,EventName,EventAbstract,EventFromDate,EventToDate,EventVenue,RegLink,MaxCapacity,ScEligibility,RegCapacity,PubtoMSCOM,PostRegistered,PostAttended,PostManualOverallRating,EventOwner,EventStatus,EventVisibility,EventType,CityName")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityName = new SelectList(db.Cities, "CityName", "State", @event.CityName);
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                 }
           
                Event event1 = db.Events.Find(id);
                if (event1 == null)
                {
                    return HttpNotFound();
                }
                return View(event1);
          }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Event event1 = db.Events.Find(id);
                if (event1.FollowedByUsers.Count == 0)
                {
                    //Delete child Event Tracks.
                    if(event1.EventTracks != null)
                    {
                        foreach (var existingChild in event1.EventTracks.ToList())
                        {
                            //Delete child Track Agendas.
                            existingChild.TrackAgendas.ToList().ForEach(p => db.TrackAgendas.Remove(p));
                            db.EventTracks.Remove(existingChild);
                        }
                    }
                    //Delete Event.
                    db.Events.Remove(event1);
                    db.SaveChanges();
                }
                else
                {
                    ResourceManager rm = new ResourceManager(typeof(ErrorMessages));

                    ViewBag.showDeleteMsg = rm.GetString("DeleteMsg").Trim('"') + " message: this event followed by users.";
                    return View(event1);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("Event Delete Error: " + e);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
