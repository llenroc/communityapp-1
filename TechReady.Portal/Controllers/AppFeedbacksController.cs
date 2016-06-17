using System.Linq;
using System.Web.Mvc;
using TechReady.Portal.Models;

namespace TechReady.Portal.Controllers
{
    [Authorize]
    public class AppFeedbacksController : Controller
    {
        private TechReadyDbContext db = new TechReadyDbContext();

        // GET: AppFeedbacks
        public ActionResult Index()
        {
            var appFeedbacks = db.AppFeedbacks.ToList();
            return View(appFeedbacks);
        }
    }
}