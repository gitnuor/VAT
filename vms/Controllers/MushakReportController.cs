using Microsoft.AspNetCore.Mvc;

namespace vms.Controllers;

public class MushakReportController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}