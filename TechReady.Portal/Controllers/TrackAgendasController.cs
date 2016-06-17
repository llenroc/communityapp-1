using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TechReady.Portal.Models;

namespace TechReady.Portal.Controllers
{
    [Authorize]
    public class TrackAgendasController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: TrackAgendas
        public ActionResult Index()
        {
            var trackAgendas = db.TrackAgendas.Include(t => t.EventTrack).Include(t => t.Session).Include(t => t.Speaker);
            return View(trackAgendas.ToList());
        }

        // GET: TrackAgendas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackAgenda trackAgenda = db.TrackAgendas.Find(id);
            if (trackAgenda == null)
            {
                return HttpNotFound();
            }
            return View(trackAgenda);
        }



        public ActionResult DisplaySession(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sId = db.TrackAgendas.Where(e => e.EventTrackID == id);  // pick up session id correspondent to events id

            //ViewBag.agendaOFThisTrack = sId.Select(e => e.EventTrack.Track.TrackDisplayName).FirstOrDefault();
           
            ViewBag.trackName = db.EventTracks.Where(e=>e.EventTrackID == id).Select(e => e.Track.TrackDisplayName).FirstOrDefault();
            ViewBag.eventName = db.EventTracks.Where(e => e.EventTrackID == id).Select(e => e.Event.EventName).FirstOrDefault();
            ViewBag.eventTrackID = id;
            return View(sId.ToList());
        }


        // GET: TrackAgendas/Create
        public ActionResult Create(int? id)  //this ID is the selected EventTrackID
        {
            ViewBag.trackName = db.EventTracks.Where(e => e.EventTrackID == id).Select(e => e.Track.TrackDisplayName).FirstOrDefault();
            ViewBag.eventName = db.EventTracks.Where(e => e.EventTrackID == id).Select(e => e.Event.EventName).FirstOrDefault();
            
            var tid=db.EventTracks.Where(e=>e.EventTrackID==id).Select(e=>e.TrackID).FirstOrDefault();

            ViewBag.SessionID = new SelectList(db.Sessions.Where(e=>e.TrackID==tid), "SessionID", "Title");
            ViewBag.SpeakerID = new SelectList(db.Speakers, "SpeakerID", "SpeakerName");
            return View();
           
        }

           // POST: TrackAgendas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, [Bind(Include = "TrackAgendaID,EventTrackID,SpeakerID,SessionID,Event_Track_Session_Speakers,StartTime,EndTime,FavCount,QRCode")] TrackAgenda trackAgenda)
        {
            trackAgenda.EventTrackID = id;
            if (ModelState.IsValid)
            {
                db.TrackAgendas.Add(trackAgenda);
                db.SaveChanges();
                return RedirectToAction("DisplaySession", new { id=trackAgenda.EventTrackID});
            }

            ViewBag.EventTrackID = new SelectList(db.EventTracks, "EventTrackID", "Event_TrackCombination", trackAgenda.EventTrackID);
          
            var tid = db.EventTracks.Where(e => e.EventTrackID == id).Select(e => e.TrackID).FirstOrDefault();

            ViewBag.SessionID = new SelectList(db.Sessions.Where(e => e.TrackID == tid), "SessionID", "Title");
            ViewBag.SpeakerID = new SelectList(db.Speakers, "SpeakerID", "SpeakerName", trackAgenda.SpeakerID);
            return View(trackAgenda);
           
        }


        // GET: TrackAgendas/Create
        //public ActionResult Create()
        //{
        //    ViewBag.EventTrackID = new SelectList(db.EventTracks, "EventTrackID", "Event_TrackCombination");
        //    ViewBag.SessionID = new SelectList(db.Sessions, "SessionID", "Title");
        //    ViewBag.SpeakerID = new SelectList(db.Speakers, "SpeakerID", "SpeakerName");
        //    return View();
        //}

        // POST: TrackAgendas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "TrackAgendaID,EventTrackID,SpeakerID,SessionID,Event_Track_Session_Speakers,StartTime,EndTime,FavCount,QRCode")] TrackAgenda trackAgenda)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.TrackAgendas.Add(trackAgenda);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.EventTrackID = new SelectList(db.EventTracks, "EventTrackID", "Event_TrackCombination", trackAgenda.EventTrackID);
        //    ViewBag.SessionID = new SelectList(db.Sessions, "SessionID", "Title", trackAgenda.SessionID);
        //    ViewBag.SpeakerID = new SelectList(db.Speakers, "SpeakerID", "SpeakerName", trackAgenda.SpeakerID);
        //    return View(trackAgenda);
        //}



        ////////////////////////////////
        
        //////////THIS HAS TO BE MADE LIKE CREATE ABOVE/////////////////////////////////////////////////////
        
        ////////////////////////////////



        // GET: TrackAgendas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackAgenda trackAgenda = db.TrackAgendas.Find(id);
            if (trackAgenda == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventTrackID = new SelectList(db.EventTracks, "EventTrackID", "TrackVenue", trackAgenda.EventTrackID);
            ViewBag.SessionID = new SelectList(db.Sessions, "SessionID", "Title", trackAgenda.SessionID);
            ViewBag.SpeakerID = new SelectList(db.Speakers, "SpeakerID", "SpeakerName", trackAgenda.SpeakerID);
            return View(trackAgenda);
        }

        // POST: TrackAgendas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrackAgendaID,EventTrackID,SpeakerID,SessionID,StartTime,EndTime,FavCount,QRCode")] TrackAgenda trackAgenda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trackAgenda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventTrackID = new SelectList(db.EventTracks, "EventTrackID", "TrackVenue", trackAgenda.EventTrackID);
            ViewBag.SessionID = new SelectList(db.Sessions, "SessionID", "Title", trackAgenda.SessionID);
            ViewBag.SpeakerID = new SelectList(db.Speakers, "SpeakerID", "SpeakerName", trackAgenda.SpeakerID);
            return View(trackAgenda);
        }

        // GET: TrackAgendas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrackAgenda trackAgenda = db.TrackAgendas.Find(id);
            if (trackAgenda == null)
            {
                return HttpNotFound();
            }
            return View(trackAgenda);
        }

        // POST: TrackAgendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           
        
                TrackAgenda trackAgenda = db.TrackAgendas.Find(id);
                db.TrackAgendas.Remove(trackAgenda);
                db.SaveChanges();
                return RedirectToAction("/DisplaySession/11");
            
           
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
