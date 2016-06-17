using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Resources;
using System.Web.Mvc;
using TechReady.Portal.Models;

namespace TechReady.Portal.Controllers
{
    [Authorize]
    public class AudienceOrgsController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: AudienceOrgs
        public ActionResult Index()
        {
            var audienceOrgs = db.AudienceOrgs.Include(a => a.AudienceType1);
            return View(audienceOrgs.ToList());
        }

        // GET: AudienceOrgs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AudienceOrg audienceOrg = db.AudienceOrgs.Find(id);
            if (audienceOrg == null)
            {
                return HttpNotFound();
            }
            return View(audienceOrg);
        }

        // GET: AudienceOrgs/Create
        public ActionResult Create()
        {
            ViewBag.AudienceTypeID = new SelectList(db.AudienceTypes, "AudienceTypeID", "TypeOfAudience");
            return View();
        }

        // POST: AudienceOrgs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AudienceOrgID,AudOrg,AudienceTypeID")] AudienceOrg audienceOrg)
        {
            if (ModelState.IsValid)
            {
                db.AudienceOrgs.Add(audienceOrg);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AudienceTypeID = new SelectList(db.AudienceTypes, "AudienceTypeID", "TypeOfAudience", audienceOrg.AudienceTypeID);
            return View(audienceOrg);
        }

        // GET: AudienceOrgs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AudienceOrg audienceOrg = db.AudienceOrgs.Find(id);
            if (audienceOrg == null)
            {
                return HttpNotFound();
            }
            ViewBag.AudienceTypeID = new SelectList(db.AudienceTypes, "AudienceTypeID", "TypeOfAudience", audienceOrg.AudienceTypeID);
            return View(audienceOrg);
        }

        // POST: AudienceOrgs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AudienceOrgID,AudOrg,AudienceTypeID")] AudienceOrg audienceOrg)
        {
            if (ModelState.IsValid)
            {
                db.Entry(audienceOrg).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AudienceTypeID = new SelectList(db.AudienceTypes, "AudienceTypeID", "TypeOfAudience", audienceOrg.AudienceTypeID);
            return View(audienceOrg);
        }

        // GET: AudienceOrgs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AudienceOrg audienceOrg = db.AudienceOrgs.Find(id);
            if (audienceOrg == null)
            {
                return HttpNotFound();
            }
            return View(audienceOrg);
        }

        // POST: AudienceOrgs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AudienceOrg audienceOrg = db.AudienceOrgs.Find(id);
            var appUsers = db.AppUsers.Where(a => a.AudienceOrgID == audienceOrg.AudienceOrgID).FirstOrDefault();
            if (appUsers == null)
            {
                db.AudienceOrgs.Remove(audienceOrg);
                db.SaveChanges();
            }
            else
            {
                ResourceManager rm = new ResourceManager(typeof(ErrorMessages));

                ViewBag.showDeleteMsg = rm.GetString("DeleteMsg").Trim('"') + " message: this audience organization is refrenced to app users";
                return View(audienceOrg);
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
