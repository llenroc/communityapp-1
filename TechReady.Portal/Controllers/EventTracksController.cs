using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TechReady.Portal.Models;

namespace TechReady.Portal.Controllers
{
    [Authorize]
    public class EventTracksController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: EventTracks
        public ActionResult Index()
        {
            var eventTracks = db.EventTracks.Include(e => e.Event).Include(e => e.Track);
            return View(eventTracks.ToList());
        }

        // GET: EventTracks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventTrack eventTrack = db.EventTracks.Find(id);
            if (eventTrack == null)
            {
                return HttpNotFound();
            }
            return View(eventTrack);
        }

        public ActionResult Browse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tracksOfThisEvent = db.EventTracks.Where(e => e.EventID == id);
            //ViewBag.selectedEvent = tracksOfThisEvent.Select(e => e.Event.EventName).FirstOrDefault();
            ViewBag.selectedEvent = db.Events.Where(e => e.EventID == id).Select(e => e.EventName).FirstOrDefault();
            ViewBag.selectedEventID = db.Events.Where(e => e.EventID == id).Select(e => e.EventID).FirstOrDefault();

            return View(tracksOfThisEvent.ToList());
        }



        // GET: EventTracks/Create
        public ActionResult Create()
        {
            ViewBag.EventID = new SelectList(db.Events, "EventID", "EventName");
            ViewBag.TrackID = new SelectList(db.Tracks, "TrackID", "TrackDisplayName");
            //return View();
            return RedirectToAction("/Browse/6");
        }

        // POST: EventTracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventTrackID,EventID,TrackID,TrackVenue,TrackDate,TrackStartTime,TrackEndTime,TrackSeating,TrackOwner")] EventTrack eventTrack)
        {
            if (ModelState.IsValid)
            {
                db.EventTracks.Add(eventTrack);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventID = new SelectList(db.Events, "EventID", "EventName", eventTrack.EventID);
            ViewBag.TrackID = new SelectList(db.Tracks, "TrackID", "TrackDisplayName", eventTrack.TrackID);
            return View(eventTrack);
        }


        ///////////////////////////////////////////////////////////////////////////////////////
        //Now we're editing Create method to show only the relevant Event in the Event dropdown
        //These are called from Browse view of EventTracks
        ///////////////////////////////////////////////////////////////////////////////////////


        // GET: EventTracks/Create
        public ActionResult AddTracks(int? id)
        {
            ViewBag.EventID = new SelectList(db.Events.Where(e=>e.EventID==id), "EventID", "EventName");
            ViewBag.TrackID = new SelectList(db.Tracks, "TrackID", "TrackDisplayName");
            ViewBag.selectedEvent = db.Events.Where(e => e.EventID == id).Select(e => e.EventName).FirstOrDefault();

            return View();
        }

        // POST: EventTracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTracks(int? id, [Bind(Include = "EventTrackID,EventID,TrackID,TrackVenue,TrackStartTime,TrackEndTime,TrackSeating,TrackOwner")] EventTrack eventTrack)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.EventTracks.Add(eventTrack);
                    db.SaveChanges();
                    return RedirectToAction("Browse", new { id = eventTrack.EventID });
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return RedirectToAction("Browse", new { id = eventTrack.EventID });
                }
              
            }

            ViewBag.EventID = new SelectList(db.Events.Where(e => e.EventID == id), "EventID", "EventName", eventTrack.EventID);
            ViewBag.TrackID = new SelectList(db.Tracks, "TrackID", "TrackDisplayName", eventTrack.TrackID);
            return View(eventTrack);
        }





        // GET: EventTracks/Edit/5
        public ActionResult Edit(int? id, int? eventid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventTrack eventTrack = db.EventTracks.Find(id);
            if (eventTrack == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventID = new SelectList(db.Events.Where(e=>e.EventID==eventid), "EventID", "EventName", eventTrack.EventID);
            ViewBag.TrackID = new SelectList(db.Tracks, "TrackID", "TrackDisplayName", eventTrack.TrackID);
            return View(eventTrack);
        }

        // POST: EventTracks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventTrackID,EventID,TrackID,TrackVenue,TrackDate,TrackStartTime,TrackEndTime,TrackSeating,TrackOwner")] EventTrack eventTrack)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventTrack).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Browse", new { id = eventTrack.EventID });
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "EventName", eventTrack.EventID);
            ViewBag.TrackID = new SelectList(db.Tracks, "TrackID", "TrackDisplayName", eventTrack.TrackID);
            return View(eventTrack);
        }

        // GET: EventTracks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventTrack eventTrack = db.EventTracks.Find(id);
            if (eventTrack == null)
            {
                return HttpNotFound();
            }
            return View(eventTrack);
        }

        // POST: EventTracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                EventTrack eventTrack = db.EventTracks.Find(id);
                db.EventTracks.Remove(eventTrack);
                db.SaveChanges();
                return RedirectToAction("/Browse/6");
            }
            catch(Exception e)
            {

            }
            return RedirectToAction("/Browse/6");
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
