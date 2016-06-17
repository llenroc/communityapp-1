using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Resources;
using System.Web.Mvc;
using TechReady.Portal.Models;

namespace TechReady.Portal.Controllers
{
    [Authorize]
    public class SecondaryTechnologiesController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: SecondaryTechnologies
        public ActionResult Index()
        {
            var secondaryTechnologies = db.SecondaryTechnologies.Include(s => s.PrimaryTechnology);
            return View(secondaryTechnologies.ToList());
        }

        // GET: SecondaryTechnologies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecondaryTechnology secondaryTechnology = db.SecondaryTechnologies.Find(id);
            if (secondaryTechnology == null)
            {
                return HttpNotFound();
            }
            return View(secondaryTechnology);
        }

        // GET: SecondaryTechnologies/Create
        public ActionResult Create()
        {
            ViewBag.PrimaryTechnologyID = new SelectList(db.PrimaryTechnologies, "PrimaryTechnologyID", "PrimaryTech");
            return View();
        }

        // POST: SecondaryTechnologies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SecondaryTechnologyID,SecondaryTech,PrimaryTechnologyID")] SecondaryTechnology secondaryTechnology)
        {
            if (ModelState.IsValid)
            {
                if(!db.SecondaryTechnologies.Any(x => x.SecondaryTech == secondaryTechnology.SecondaryTech))
                {
                    db.SecondaryTechnologies.Add(secondaryTechnology);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("SecondaryTechnology", "Duplicate values are not allowed");
                    ViewBag.PrimaryTechnologyID = new SelectList(db.PrimaryTechnologies, "PrimaryTechnologyID", "PrimaryTech", secondaryTechnology.PrimaryTechnologyID);
                    return View();
                }
            }

            ViewBag.PrimaryTechnologyID = new SelectList(db.PrimaryTechnologies, "PrimaryTechnologyID", "PrimaryTech", secondaryTechnology.PrimaryTechnologyID);
            return View(secondaryTechnology);
        }

        // GET: SecondaryTechnologies/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SecondaryTechnology secondaryTechnology = db.SecondaryTechnologies.Find(id);
        //    if (secondaryTechnology == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.PrimaryTechnologyID = new SelectList(db.PrimaryTechnologies, "PrimaryTechnologyID", "PrimaryTech", secondaryTechnology.PrimaryTechnologyID);
        //    return View(secondaryTechnology);
        //}

        //// POST: SecondaryTechnologies/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "SecondaryTechnologyID,SecondaryTech,PrimaryTechnologyID")] SecondaryTechnology secondaryTechnology)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(secondaryTechnology).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.PrimaryTechnologyID = new SelectList(db.PrimaryTechnologies, "PrimaryTechnologyID", "PrimaryTech", secondaryTechnology.PrimaryTechnologyID);
        //    return View(secondaryTechnology);
        //}

        // GET: SecondaryTechnologies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecondaryTechnology secondaryTechnology = db.SecondaryTechnologies.Find(id);
            if (secondaryTechnology == null)
            {
                return HttpNotFound();
            }
            return View(secondaryTechnology);
        }

        // POST: SecondaryTechnologies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SecondaryTechnology secondaryTechnology = db.SecondaryTechnologies.Find(id);
            if (secondaryTechnology != null)
            {
                var sessions = db.Sessions.Where(s => s.SecondaryTechnologyID == secondaryTechnology.SecondaryTechnologyID).FirstOrDefault();
                if (sessions == null)
                {
                    db.SecondaryTechnologies.Remove(secondaryTechnology);
                    db.SaveChanges();
                }
                else
                {
                    ResourceManager rm = new ResourceManager(typeof(ErrorMessages));

                    ViewBag.showDeleteMsg = rm.GetString("DeleteMsg").Trim('"') + " message: this secondary technology is refrenced to sessions.";
                    return View(secondaryTechnology);
                }
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
