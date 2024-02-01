using Microsoft.AspNetCore.Mvc;

namespace vms.Controllers;

public class SaleOrderController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}