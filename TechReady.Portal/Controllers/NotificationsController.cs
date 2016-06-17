using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TechReady.Portal.Helpers;
using TechReady.Portal.Models;
using Notification = TechReady.Portal.Models.Notification;

namespace TechReady.Portal.Controllers
{
    [Authorize]

    public class NotificationsController : Controller
    {

        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: Notifications
        public async Task<ActionResult> Index()
        {
            return View(await db.Notifications.ToListAsync());
        }

        // GET: Notifications/Create
        public ActionResult Create()
        {
            Dictionary<string,string> dic = new Dictionary<string, string>();
            
            
           
            dic.Add("Announcement", "Announcement");

            dic.Add("Link", "Link");

            SelectList itemList = new SelectList(dic,"key","value");

            ViewBag.NotificationList = itemList;
            ViewBag.PrimaryTechnologies = new MultiSelectList(db.PrimaryTechnologies, "PrimaryTech", "PrimaryTech");
            ViewBag.AudienceTypes = new MultiSelectList(db.AudienceTypes, "TypeOfAudience", "TypeOfAudience");


            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "NotificationID,TypeOfNotification,ResourceId,ActionLink,NotificationTitle,NotificationMessage,PushDateTime")] Notification notification,
            List<string> TechTagNames,List<string> AudienceTypeNames,string NotificationType)
            
        {
            if (ModelState.IsValid)
            {
                notification.PushDateTime = DateTime.UtcNow;
                notification.TypeOfNotification = (NotificationType)Enum.Parse(typeof(NotificationType), NotificationType, true);
                db.Notifications.Add(notification);

                notification.TechTags = new List<PrimaryTechnology>();
                notification.AudienceTypeTags = new List<AudienceType>();
                notification.SentToUsers = new List<AppUserNotificationAction>();

                if (TechTagNames != null)
                {
                
                    foreach (var techTag in TechTagNames)
                    {
                        //primary technology from client.
                        var primaryTech = db.PrimaryTechnologies.FirstOrDefault(x => x.PrimaryTech == techTag);
                        if (primaryTech != null)
                        {
                            notification.TechTags.Add(primaryTech);
                        }

                        var appUsers = db.AppUsers.Where(p => p.TechnologyTags.Any(c => primaryTech.PrimaryTechnologyID == c.PrimaryTechnologyID));
                        foreach (var user in appUsers)
                        {
                            //add app users to this notification. 
                            if (notification.SentToUsers.FirstOrDefault(x => x.AppUserID == user.AppUserID) == null)
                            {
                                notification.SentToUsers.Add(
                                    new AppUserNotificationAction()
                                    {
                                        AppUserID = user.AppUserID,
                                        Read = false

                                    });
                            }
                        }

                    }
                }

                if (AudienceTypeNames != null)
                {
                    //add audience type names.
                    foreach (var audTypeTag  in AudienceTypeNames)
                    {
                        var audType = db.AudienceTypes.FirstOrDefault(x => x.TypeOfAudience == audTypeTag);
                        if (audType != null)
                        {
                            notification.AudienceTypeTags.Add(audType);
                        }

                        var appUsers = db.AppUsers.Where(p => p.AudienceOrg.AudienceTypeID== audType.AudienceTypeID);
                        foreach (var user in appUsers)
                        {
                            //no app user exist to send this notification.
                            if (notification.SentToUsers.FirstOrDefault(x => x.AppUserID == user.AppUserID) == null)
                            {
                                notification.SentToUsers.Add(
                                    new AppUserNotificationAction()
                                    {
                                        AppUserID = user.AppUserID,
                                        Read = false
                                    });
                            }
                        }
                    }
                }
                await db.SaveChangesAsync();
                if (TechTagNames != null)
                    if (AudienceTypeNames != null)
                      await  NotificationsHubHelper.SendNotificationAsync( TechTagNames.Union(AudienceTypeNames).Distinct().ToList().Select(x=>x.Replace(" ","_")).ToList(), notification);

                return RedirectToAction("Index");
            }

            return View(notification);
        }


  

        // GET: Notifications/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = await db.Notifications.FindAsync(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
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
