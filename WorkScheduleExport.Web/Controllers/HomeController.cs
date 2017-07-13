using Microsoft.AspNetCore.Mvc;
using System;
using TimeCare.WorkSchedule;
using WorkScheduleExport.Web.Infrastructure;
using WorkScheduleExport.Web.Infrastructure.Delivery;
using WorkScheduleExport.Web.Infrastructure.Export;
using WorkScheduleExport.Web.Models;

namespace WorkScheduleExport.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWorkScheduleExporter workScheduleExporter;
        private readonly IWorkScheduleReaderFactory workScheduleReaderFactory;
        private readonly IWorkScheduleDeliveryService deliveryService;

        public HomeController(IWorkScheduleReaderFactory workScheduleReaderFactory, IWorkScheduleExporter workScheduleExporter, IWorkScheduleDeliveryService deliveryService)
        {
            if (workScheduleReaderFactory == null)
                throw new ArgumentNullException(nameof(workScheduleReaderFactory));

            if (workScheduleExporter == null)
                throw new ArgumentNullException(nameof(workScheduleExporter));

            if (deliveryService == null)
                throw new ArgumentNullException(nameof(deliveryService));

            this.workScheduleReaderFactory = workScheduleReaderFactory;
            this.workScheduleExporter = workScheduleExporter;
            this.deliveryService = deliveryService;
        }

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

            IWorkScheduleReader workScheduleReader = workScheduleReaderFactory.Create(model.SchemaFile.OpenReadStream());
            WorkSchedule workSchedule = workScheduleReader.Read();

            byte[] exportedSchema = workScheduleExporter.Export(workSchedule);

            if (model.ContainsEmail)
            {
                deliveryService.Deliver(workSchedule, exportedSchema, new WorkScheduleDeliveryOptions { Target = model.Email });

                ViewBag.SuccessMessage = "Ditt schema har nu skickats till din e-post";

                return View();
            }

            return new FileContentResult(exportedSchema, "text/calendar") { FileDownloadName = "Arbetsschema.ics" };
        }
    }
}
