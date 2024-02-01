using Microsoft.AspNetCore.Mvc;

namespace vms.Controllers.Purchase
{
    public class PurchaseCancellationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult PurchaseCancellations()
        {
            return View();
        }

        public IActionResult AccountsView()
        {
            return View();
        }
        public ActionResult PurchaseCancellationsOrder()
        {
            return View();
        }
    }
}