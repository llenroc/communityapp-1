using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TechReady.Portal.Models;

namespace TechReady.Portal.Controllers
{
    [Authorize]
    public class FeedbacksController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: Feedbacks
        public ActionResult Index()
        {
            var feedbacks = db.Feedbacks.Include(f => f.TrackAgenda);
            return View(feedbacks.ToList());
        }

        // GET: Feedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // GET: Feedbacks/Create
        public ActionResult Create()
        {
            ////////////////////////////////////////////////////
            /////////////////////////////////////To be rectified - QR code should not come. It was Event_Track_Session_Speakers before
            //////////////////////////////////////////////////////


            ViewBag.TrackAgendaID = new SelectList(db.TrackAgendas, "TrackAgendaID", "QRCode");



            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FeedbackID,TrackAgendaID,NameToken,ContentRating,SpeakerRating,OverallRating,LTRRating,Review")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

   //         ViewBag.TrackAgendaID = new SelectList(db.TrackAgendas, "TrackAgendaID", "Event_Track_Session_Speakers", feedback.TrackAgendaID);
            return View(feedback);
        }

        // GET: Feedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
       //     ViewBag.TrackAgendaID = new SelectList(db.TrackAgendas, "TrackAgendaID", "Event_Track_Session_Speakers", feedback.TrackAgendaID);
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FeedbackID,TrackAgendaID,NameToken,ContentRating,SpeakerRating,OverallRating,LTRRating,Review")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        //    ViewBag.TrackAgendaID = new SelectList(db.TrackAgendas, "TrackAgendaID", "Event_Track_Session_Speakers", feedback.TrackAgendaID);
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedback);
            db.SaveChanges();
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
