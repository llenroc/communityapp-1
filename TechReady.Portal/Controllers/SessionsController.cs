using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Resources;
using System.Web.Mvc;
using TechReady.Portal.Models;

namespace TechReady.Portal.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: Sessions
        public ActionResult Index()
        {
            ViewBag.Tracks = db.Tracks.ToList();
            var sessions = db.Sessions.Include(s => s.Track);
            return View(sessions.ToList());
        }



        //public JsonResult GetSecondaryTechnology(int? primaryTechvalue)
        //{
        //    string sample = "Hey";
        //    var secondarytech = //db.SecondaryTechnologies.Where(string.Compare(e=>e.PrimaryTech,primaryTechvalue)==0).Select(e=>e.SecondaryTech);
        //        from s in db.SecondaryTechnologies
        //        where s.PrimaryTechnologyID == primaryTechvalue
        //        select s;
        //    return Json(new SelectList(secondarytech.ToArray(), "SecondaryTechnologyID", "SecondaryTech"), JsonRequestBehavior.AllowGet);
        //    return Json(primaryTechvalue, JsonRequestBehavior.AllowGet);
        //    return Json(new SelectList(db.SecondaryTechnologies.Where(s => s.PrimaryTechnologyID == primaryTechvalue).ToArray(), "SecondaryTechnologyID", "SecondaryTech"), JsonRequestBehavior.AllowGet);

        //}

        //public ActionResult GetSecondary(int primaryTechnologyid)
        //{
        //    var secondaryTechnologies = db.SecondaryTechnologies.Where(e=>e.PrimaryTechnologyID==primaryTechnologyid).Select(e);
        //    //var value="";
        //    //var text="";
        //    var options="";
        //    foreach(SecondaryTechnology s in secondaryTechnologies)
        //    {
        //        options += "<option value='" + s.SecondaryTechnologyID + "'>" + s.SecondaryTech + "</option>";
        //    }
        //    return(options);
        //}


        public ActionResult Browse(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sessionsOfThisTrack = db.Sessions.Where(e => e.TrackID == id);
            ViewBag.selectedTrack = db.Tracks.Where(e => e.TrackID == id).Select(e => e.TrackDisplayName).FirstOrDefault();
            ViewBag.selectedTrackID = id;
            return View(sessionsOfThisTrack.ToList());
        }

        // GET: Sessions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // GET: Sessions/Create
        public ActionResult Create(int id)
        {
            ViewBag.TrackID = new SelectList(db.Tracks.Where(e => e.TrackID == id), "TrackID", "TrackDisplayName");
            ViewBag.PrimaryTechnologyID = new SelectList(db.PrimaryTechnologies,"PrimaryTechnologyID", "PrimaryTech");
            ViewBag.SecondaryTechnologyID = new SelectList(db.SecondaryTechnologies, "SecondaryTechnologyID", "SecondaryTech");

            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, [Bind(Include = "SessionID,SessionNo,Title,Abstract,TechLevel,PrimaryTechnologyID,SecondaryTechnologyID,Product,InfraNeeds,PreRequisites,PostRequisites,TrackID")] Session session)
        {
            if (ModelState.IsValid)
            {
                db.Sessions.Add(session);
                db.SaveChanges();
                return RedirectToAction("Browse", new { id=session.TrackID});
            }

            ViewBag.TrackID = new SelectList(db.Tracks.Where(e=>e.TrackID==id), "TrackID", "TrackDisplayName", session.TrackID);
            ViewBag.PrimaryTechnologyID = new SelectList(db.PrimaryTechnologies, "PrimaryTechnologyID", "PrimaryTech");
            return View(session);
        }

        // GET: Sessions/Edit/5
        public ActionResult Edit(int? id, int trackID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            ViewBag.TrackID = new SelectList(db.Tracks.Where(e=>e.TrackID==trackID), "TrackID", "TrackDisplayName", session.TrackID);
            ViewBag.PrimaryTechnologyID = new SelectList(db.PrimaryTechnologies, "PrimaryTechnologyID", "PrimaryTech");
            ViewBag.SecondaryTechnologyID = new SelectList(db.SecondaryTechnologies, "SecondaryTechnologyID", "SecondaryTech");

            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SessionID,SessionNo,Title,Abstract,TechLevel,PrimaryTechnologyID,SecondaryTechnologyID,Product,InfraNeeds,PreRequisites,PostRequisites,TrackID")] Session session)
        {
            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TrackID = new SelectList(db.Tracks, "TrackID", "TrackDisplayName", session.TrackID);
            ViewBag.PrimaryTech = new SelectList(db.PrimaryTechnologies, "PrimaryTech", "PrimaryTech");

            return View(session);
        }

        // GET: Sessions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Session session = db.Sessions.Find(id);
            var trackAgendas = session.TrackAgenda.FirstOrDefault();
            if (trackAgendas == null)
            {
                db.Sessions.Remove(session);
                db.SaveChanges();
            }
            else
            {
                ResourceManager rm = new ResourceManager(typeof(ErrorMessages));

                ViewBag.showDeleteMsg = rm.GetString("DeleteMsg").Trim('"') + " message: this session is refrenced to event track agendas.";
                return View(session);
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
