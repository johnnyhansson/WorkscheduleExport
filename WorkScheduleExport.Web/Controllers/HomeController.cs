using Microsoft.AspNetCore.Mvc;
using WorkScheduleExport.Web.Models;

namespace WorkScheduleExport.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(WorkScheduleExportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            return View();
        }
    }
}
