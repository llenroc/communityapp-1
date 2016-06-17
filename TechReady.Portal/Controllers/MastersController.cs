using System.Web.Mvc;

namespace TechReady.Portal.Controllers
{
    [Authorize]
    public class MastersController : Controller
    {
        // GET: Masters
        public ActionResult Index()
        {
            return View();
        }
    }
}