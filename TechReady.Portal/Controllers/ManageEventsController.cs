using System.Web.Mvc;

namespace TechReady.Portal.Controllers
{
    [Authorize]
    public class ManageEventsController : Controller
    {
        // GET: ManageEvents
        public ActionResult Index()
        {
            
            return RedirectToAction("CurrentEvents", "Events");
        }
    }
}