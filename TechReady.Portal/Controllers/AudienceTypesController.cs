using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Resources;
using System.Web.Mvc;
using TechReady.Portal.Models;

namespace TechReady.Portal.Controllers
{
    [Authorize]
    public class AudienceTypesController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: AudienceTypes
        public ActionResult Index()
        {
            return View(db.AudienceTypes.ToList());
        }

        // GET: AudienceTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AudienceType audienceType = db.AudienceTypes.Find(id);
            if (audienceType == null)
            {
                return HttpNotFound();
            }
            return View(audienceType);
        }

        // GET: AudienceTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AudienceTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AudienceTypeID,TypeOfAudience")] AudienceType audienceType)
        {
            if (ModelState.IsValid)
            {
                db.AudienceTypes.Add(audienceType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(audienceType);
        }

        // GET: AudienceTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AudienceType audienceType = db.AudienceTypes.Find(id);
            if (audienceType == null)
            {
                return HttpNotFound();
            }
            return View(audienceType);
        }

        // POST: AudienceTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AudienceTypeID,TypeOfAudience")] AudienceType audienceType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(audienceType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(audienceType);
        }

        // GET: AudienceTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AudienceType audienceType = db.AudienceTypes.Find(id);
            if (audienceType == null)
            {
                return HttpNotFound();
            }
            return View(audienceType);
        }

        // POST: AudienceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AudienceType audienceType = db.AudienceTypes.Find(id);

            var audienceOrgs = audienceType.AudienceOrg.FirstOrDefault();
            var notifications = audienceType.Notifications.FirstOrDefault();
            var learningResourceFeeds = audienceType.LearningResourceFeeds.FirstOrDefault();
            var learningResources = audienceType.LearningResources.FirstOrDefault();

            if (audienceOrgs == null && notifications == null && learningResourceFeeds == null && learningResources == null)
            {
                db.AudienceTypes.Remove(audienceType);
                db.SaveChanges();
            }
            else
            {
                ResourceManager rm = new ResourceManager(typeof(ErrorMessages));

                ViewBag.showDeleteMsg = rm.GetString("DeleteMsg").Trim('"') + " message: this audience type is refrenced to audience organization, notification, learning resource feeds or learning resources.";
                return View(audienceType);
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
