using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Resources;
using System.Web.Mvc;
using TechReady.Portal.Models;

namespace TechReady.Portal.Controllers
{
    [Authorize]

    public class LearningResourceFeedsController : Controller
    {

        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: LearningResourceFeeds
        public ActionResult Index()
        {
            var learningResourceFeeds = db.LearningResourceFeeds.Include(lr => lr.PrimaryTechnology).Include(lr => lr.AudienceTypes).Include(lr => lr.LearningResourceType);
            return View(learningResourceFeeds.ToList());
        }
        
        // GET: LearningResourceFeeds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //details learning resource feed.
            var learningResourceFeed = db.LearningResourceFeeds.Where(lr=>lr.LearningResourceFeedID==id).Include(lr => lr.PrimaryTechnology).Include(lr => lr.AudienceTypes).Include(lr => lr.LearningResourceType).FirstOrDefault();
            if (learningResourceFeed == null)
            {
                return HttpNotFound();
            }
            return View(learningResourceFeed);
        }

        // GET: LearningResourceFeeds/Create
        public ActionResult Create()
        {
            ViewBag.PrimaryTechnologyID = new SelectList(db.PrimaryTechnologies, "PrimaryTechnologyID","PrimaryTech");
            ViewBag.AudienceTypes = new MultiSelectList(db.AudienceTypes, "TypeOfAudience", "TypeOfAudience");
            ViewBag.LearningResourceTypeID = new SelectList(db.LearningResourceTypes, "LearningResourceTypeID", "LearningResourceTypeName");
            return View();
        }

        // POST: LearningResourceFeeds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrimaryTechnologyID,AudienceTypes,LearningResourceTypeID,RSSLink")] LearningResourceFeed learningResourceFeed,
            List<string> AudienceTypeNames)
        {
            if (ModelState.IsValid)
            {
                    learningResourceFeed.AudienceTypes = new List<AudienceType>();
                if (AudienceTypeNames != null)
                {
                    foreach (var audienceTypeName in AudienceTypeNames)
                    {
                        //add new audience type to learning resource.
                        var audienceType = db.AudienceTypes.FirstOrDefault(a => a.TypeOfAudience == audienceTypeName);
                        if (audienceType != null)
                        {
                            learningResourceFeed.AudienceTypes.Add(audienceType);
                        }
                    }
                }
                    //add new learning resource.
                    db.LearningResourceFeeds.Add(learningResourceFeed);
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }

            return View(learningResourceFeed);
        }

        // GET: LearningResourceFeeds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var learningResourceFeed = db.LearningResourceFeeds.Where(lr => lr.LearningResourceFeedID == id).Include(lr => lr.PrimaryTechnology).Include(lr => lr.AudienceTypes).Include(lr => lr.LearningResourceType).FirstOrDefault();

            if (learningResourceFeed == null)
            {
                return HttpNotFound();
            }
            return View(learningResourceFeed);
        }

        // POST: LearningResourceFeeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LearningResourceFeed learningResourceFeed = db.LearningResourceFeeds.Find(id);
            var audienceTypes = learningResourceFeed.AudienceTypes.FirstOrDefault();
            if (audienceTypes == null)
            {
                db.LearningResourceFeeds.Remove(learningResourceFeed);
                db.SaveChanges();
            }
            else
            {
                ResourceManager rm = new ResourceManager(typeof(ErrorMessages));
                //delete error message from resource file.
                ViewBag.showDeleteMsg = rm.GetString("DeleteMsg").Trim('"') + " message: this learning resource feed is refrenced to audience types";
                return View(learningResourceFeed);
            }
            
            return RedirectToAction("Index");
        }

        // GET: LearningResourceFeeds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var learningResourceFeed = db.LearningResourceFeeds.Where(f=>f.LearningResourceFeedID == id).FirstOrDefault();

            if (learningResourceFeed == null)
            {
                return HttpNotFound();
            }
            
            var audienceTypeCount = learningResourceFeed.AudienceTypes.Count();
            string[] selectedAudience = new string[audienceTypeCount];
            int i = 0;
            foreach (var audienceType in learningResourceFeed.AudienceTypes.ToList())
            {
                selectedAudience.SetValue(audienceType.TypeOfAudience, i);              //add selected audience type to string array for multi select.
                i++;
            }
            ViewBag.PrimaryTechnologyID = new SelectList(db.PrimaryTechnologies, "PrimaryTechnologyID", "PrimaryTech",learningResourceFeed.PrimaryTechnologyID);
            ViewBag.AudienceTypeNames = new MultiSelectList(db.AudienceTypes, "TypeOfAudience", "TypeOfAudience", selectedAudience);
            ViewBag.LearningResourceTypeID = new SelectList(db.LearningResourceTypes, "LearningResourceTypeID", "LearningResourceTypeName",learningResourceFeed.LearningResourceTypeID);
            return View(learningResourceFeed);
        }

        // POST: LearningResourceFeeds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LearningResourceFeedID,PrimaryTechnologyID,AudienceTypes,LearningResourceTypeID,RSSLink")] LearningResourceFeed learningResourceFeed,
            List<string> AudienceTypeNames)
        {
            if (ModelState.IsValid)
            {
                var existingLearningResourceFeed = db.LearningResourceFeeds.Find(learningResourceFeed.LearningResourceFeedID);

                //update existing learning resource feeds.
                db.Entry(existingLearningResourceFeed).CurrentValues.SetValues(learningResourceFeed);

                if (existingLearningResourceFeed.AudienceTypes == null)
                {
                    existingLearningResourceFeed.AudienceTypes = new List<AudienceType>();
                }

                
                var existingAudienceTypeNames = existingLearningResourceFeed.AudienceTypes.Select(x=>x.TypeOfAudience).ToList();
                foreach (var audienceType in existingAudienceTypeNames)
                {
                    var existingChild = existingLearningResourceFeed.AudienceTypes.FirstOrDefault(x => x.TypeOfAudience == audienceType);
                    if (AudienceTypeNames.FirstOrDefault(x => x == audienceType) == null)
                    {
                        existingLearningResourceFeed.AudienceTypes.Remove(existingChild);           //remove existing audience types if exist and not in current audience types received.
                    }
                }

                foreach (string at in AudienceTypeNames)
                {
                    if (existingAudienceTypeNames.FirstOrDefault(x => x == at) == null)
                    {
                        //add audience type if not exist in existing.
                        var audienceType = db.AudienceTypes.FirstOrDefault(x => x.TypeOfAudience == at);

                        if (audienceType != null)
                        {
                            existingLearningResourceFeed.AudienceTypes.Add(audienceType);
                        }
                    }
                }
                
                db.SaveChanges();
                
                return RedirectToAction("Index", new { id = learningResourceFeed.LearningResourceFeedID });
            }
            ViewBag.PrimaryTechnologyID = new SelectList(db.PrimaryTechnologies, "PrimaryTechnologyID", "PrimaryTech", learningResourceFeed.PrimaryTechnologyID);
            
            ViewBag.LearningResourceTypeID = new SelectList(db.LearningResourceTypes, "LearningResourceTypeID", "LearningResourceTypeName", learningResourceFeed.LearningResourceTypeID);
            return View(learningResourceFeed);
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