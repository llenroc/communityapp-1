using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Resources;
using System.Web.Mvc;
using TechReady.Portal.Models;

namespace TechReady.Portal.Controllers
{
    [Authorize]
    public class TracksController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: Tracks
        public ActionResult Index()
        {
            var tracks = db.Tracks.Include(t => t.Theme);
            return View(tracks.ToList());
        }

        // GET: Tracks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track track = db.Tracks.Find(id);
            if (track == null)
            {
                return HttpNotFound();
            }
            return View(track);
        }

        // GET: Tracks/Create
        public ActionResult Create()
        {
            ViewBag.ThemeID = new SelectList(db.Themes, "ThemeID", "ThemeName");
            ViewBag.AudienceTypeID = new SelectList(db.AudienceTypes, "AudienceTypeID", "TypeOfAudience");
            
            return View();
        }

        // POST: Tracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrackID,TrackDisplayName,TrackAbstract,AudienceTypeID,InternalTrackName,Format,ThemeID")] Track track)
        {
            if (ModelState.IsValid)
            {
                db.Tracks.Add(track);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ThemeID = new SelectList(db.Themes, "ThemeID", "ThemeName", track.ThemeID);

            ViewBag.AudienceTypeID = new SelectList(db.AudienceTypes, "AudienceTypeID", "TypeOfAudience", track.AudienceTypeID);
            return View(track);
        }

        // GET: Tracks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track track = db.Tracks.Find(id);
            if (track == null)
            {
                return HttpNotFound();
            }
            ViewBag.ThemeID = new SelectList(db.Themes, "ThemeID", "ThemeName", track.ThemeID);
            ViewBag.AudienceTypeID = new SelectList(db.AudienceTypes, "AudienceTypeID", "TypeOfAudience", track.AudienceTypeID);

            return View(track);
        }

        // POST: Tracks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrackID,TrackDisplayName,TrackAbstract,AudienceTypeID,InternalTrackName,Format,ThemeID,AudienceTypeID")] Track track)
        {
            if (ModelState.IsValid)
            {
                db.Entry(track).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ThemeID = new SelectList(db.Themes, "ThemeID", "ThemeName", track.ThemeID);
            ViewBag.AudienceTypeID = new SelectList(db.AudienceTypes, "AudienceTypeID", "TypeOfAudience", track.AudienceTypeID);

            return View(track);
        }

        // GET: Tracks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track track = db.Tracks.Find(id);
            if (track == null)
            {
                return HttpNotFound();
            }
            return View(track);
        }

        // POST: Tracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Track track = db.Tracks.Find(id);
            var childEventTracks = track.EventTracks.FirstOrDefault();
            var childSessions = track.Session.FirstOrDefault();

            if (childEventTracks == null && childSessions == null)
            {
                db.Tracks.Remove(track);
                db.SaveChanges();
            }
            else
            {
                ResourceManager rm = new ResourceManager(typeof(ErrorMessages));

                ViewBag.showDeleteMsg = rm.GetString("DeleteMsg").Trim('"') + " message: this Track is refrenced to event tracks and sessions.";
                return View(track);
            }
            //if (childEventTracks != null)
            //{
            //    foreach(var childEventTrack in childEventTracks.ToList())
            //    {
            //        var childTrackAgendas = childEventTrack.TrackAgendas;
            //        if (childTrackAgendas != null)
            //        {
            //            foreach(var childTrackAgenda in childTrackAgendas)
            //            {
            //                db.TrackAgendas.Remove(childTrackAgenda);
            //            }
            //        }
            //        db.EventTracks.Remove(childEventTrack);
            //    }
            //}

            //var childSessions = track.Session;
            //if (childSessions != null)
            //{
            //    foreach(var session in childSessions.ToList())
            //    {
            //        var trackAgendas = session.TrackAgenda;
            //        if (trackAgendas != null)
            //        {
            //            foreach(var trackAgenda in trackAgendas.ToList())
            //            {
            //                db.TrackAgendas.Remove(trackAgenda);
            //            }
            //        }
            //        db.Sessions.Remove(session);
            //    }
            //}
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
