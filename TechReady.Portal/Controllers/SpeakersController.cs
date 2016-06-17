using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Resources;
using System.Web.Mvc;
using TechReady.Portal.Models;

// DXEvents
namespace TechReady.Portal.Controllers
{
    [Authorize]
    public class SpeakersController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: Speakers
        public ActionResult Index()
        {
            return View(db.Speakers.ToList());
        }

        // GET: Speakers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Speaker speaker = db.Speakers.Find(id);
            if (speaker == null)
            {
                return HttpNotFound();
            }
            return View(speaker);
        }

        // GET: Speakers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Speakers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SpeakerID,SpeakerName,Profile,PicUrl,Affiliation,Title,Email,MobilePhone,TwitterHandle,FacebookLink,LinkedInLink,BlogLink,Type,RecommendedBy,SpeakerLevel,Location")] Speaker speaker)
        {
            if (ModelState.IsValid)
            {
                db.Speakers.Add(speaker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(speaker);
        }

        // GET: Speakers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Speaker speaker = db.Speakers.Find(id);
            if (speaker == null)
            {
                return HttpNotFound();
            }
            return View(speaker);
        }

        // POST: Speakers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SpeakerID,SecondarySpeakerName,SpeakerName,Profile,PicUrl,Affiliation,Title,Email,MobilePhone,TwitterHandle,FacebookLink,LinkedInLink,BlogLink,Type,RecommendedBy,SpeakerLevel,Location")] Speaker speaker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(speaker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(speaker);
        }

        // GET: Speakers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Speaker speaker = db.Speakers.Find(id);
            if (speaker == null)
            {
                return HttpNotFound();
            }
            return View(speaker);
        }

        // POST: Speakers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Speaker speaker = db.Speakers.Find(id);
            if(speaker.TrackAgendas.Count == 0)
            {
                if (speaker.FollowedByUsers.Count == 0)
                {
                    db.Speakers.Remove(speaker);
                    db.SaveChanges();
                }
                else
                {
                    ResourceManager rm = new ResourceManager(typeof(ErrorMessages));

                    ViewBag.showDeleteMsg = rm.GetString("DeleteMsg").Trim('"') + " message: this speaker is followed by app users.";
                    return View(speaker);
                }
                
            }
            else
            {
                ResourceManager rm = new ResourceManager(typeof(ErrorMessages));

                ViewBag.showDeleteMsg = rm.GetString("DeleteMsg").Trim('"') + " message: this speaker is associated to track agendas.";
                return View(speaker);
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
