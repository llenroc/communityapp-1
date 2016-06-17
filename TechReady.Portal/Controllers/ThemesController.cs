using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Resources;
using System.Web.Mvc;
using TechReady.Portal.Models;

namespace TechReady.Portal.Controllers
{
    [Authorize]
    public class ThemesController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: Themes
        public ActionResult Index()
        {
            return View(db.Themes.ToList());
        }

        // GET: Themes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theme theme = db.Themes.Find(id);
            if (theme == null)
            {
                return HttpNotFound();
            }
            return View(theme);
        }

        // GET: Themes/Create
        public ActionResult Create()
        {
            return View(new Theme());
        }

        // POST: Themes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ThemeID,FY,ThemeName")] Theme theme)
        {
            if (ModelState.IsValid)
            {
                db.Themes.Add(theme);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(theme);
        }

        // GET: Themes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theme theme = db.Themes.Find(id);
            if (theme == null)
            {
                return HttpNotFound();
            }
            return View(theme);
        }

        // POST: Themes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ThemeID,ThemeName,FY")] Theme theme)
        {
            if (ModelState.IsValid)
            {
                db.Entry(theme).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(theme);
        }

        // GET: Themes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theme theme = db.Themes.Find(id);
            if (theme == null)
            {
                return HttpNotFound();
            }
            return View(theme);
        }

        // POST: Themes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Theme theme = db.Themes.Find(id);
            var childTracks = theme.Tracks.FirstOrDefault();

            if (childTracks == null)
            {
                db.Themes.Remove(theme);
                db.SaveChanges();
            }
            else
            {
                ResourceManager rm = new ResourceManager(typeof(ErrorMessages));

                ViewBag.showDeleteMsg = rm.GetString("DeleteMsg").Trim('"') + " message: this theme is refrenced to tracks.";
                return View(theme);
            }

            //if(childTracks != null)
            //{
            //    foreach(var childTrack in childTracks.ToList())
            //    {
            //        var childEventTracks = childTrack.EventTracks.ToList();
            //        if (childEventTracks != null) {
            //            foreach (var childEventTrack in childEventTracks)
            //            {
            //                var childEventTrackAgendas = childEventTrack.TrackAgendas.ToList();
            //                if (childEventTrackAgendas != null) {
            //                    foreach (var trackAgenda in childEventTrackAgendas)
            //                    {
            //                        db.TrackAgendas.Remove(trackAgenda);
            //                    }
            //                }
            //                db.EventTracks.Remove(childEventTrack);
            //            }
            //        }

            //        var childSessions = childTrack.Session.ToList();
            //        if (childSessions != null)
            //        {
            //            foreach (var session in childSessions)
            //            {
            //                var childTrackAgendas = session.TrackAgenda;
            //                if (childTrackAgendas != null)
            //                {
            //                    foreach (var trackAgenda in childTrackAgendas.ToList())
            //                    {
            //                        db.TrackAgendas.Remove(trackAgenda);
            //                    }
            //                }
            //                db.Sessions.Remove(session);
            //            }
            //        }

            //        db.Tracks.Remove(childTrack);
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
