using System.Linq;
using System.Net;
using System.Resources;
using System.Web.Mvc;
using TechReady.Portal.Models;

namespace TechReady.Portal.Controllers
{
    [Authorize]
    public class PrimaryTechnologiesController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: PrimaryTechnologies
        public ActionResult Index()
        {
            return View(db.PrimaryTechnologies.ToList());
        }

        // GET: PrimaryTechnologies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrimaryTechnology primaryTechnology = db.PrimaryTechnologies.Find(id);
            if (primaryTechnology == null)
            {
                return HttpNotFound();
            }
            return View(primaryTechnology);
        }

        // GET: PrimaryTechnologies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PrimaryTechnologies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrimaryTechnologyID,PrimaryTech")] PrimaryTechnology primaryTechnology)
        {
            if (ModelState.IsValid)
            {
                if (!db.PrimaryTechnologies.Any(x => x.PrimaryTech == primaryTechnology.PrimaryTech))
                {
                    db.PrimaryTechnologies.Add(primaryTechnology);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("PrimaryTechnology", "Duplicate values are not allowed");
                    return View();
                }
            }

            return View(primaryTechnology);
        }

        // GET: PrimaryTechnologies/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PrimaryTechnology primaryTechnology = db.PrimaryTechnologies.Find(id);
        //    if (primaryTechnology == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(primaryTechnology);
        //}

        //// POST: PrimaryTechnologies/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "PrimaryTechnologyID,PrimaryTech")] PrimaryTechnology primaryTechnology)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(primaryTechnology).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(primaryTechnology);
        //}

        // GET: PrimaryTechnologies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrimaryTechnology primaryTechnology = db.PrimaryTechnologies.Find(id);
            if (primaryTechnology == null)
            {
                return HttpNotFound();
            }
            return View(primaryTechnology);
        }

        // POST: PrimaryTechnologies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PrimaryTechnology primaryTechnology = db.PrimaryTechnologies.Find(id);
            var secondaryTechnologies = primaryTechnology.SecondaryTechnologies;
            var appUsers = primaryTechnology.AppUsers;
            var notifications = primaryTechnology.Notifications;
            if (secondaryTechnologies == null && appUsers == null && notifications == null)
            {
                db.PrimaryTechnologies.Remove(primaryTechnology);
                db.SaveChanges();
            }
            else
            {
                ResourceManager rm = new ResourceManager(typeof(ErrorMessages));

                ViewBag.showDeleteMsg = rm.GetString("DeleteMsg").Trim('"') + " message: this primary technology is refrenced to app users, secondary technology or notifications.";
                return View(primaryTechnology);
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
