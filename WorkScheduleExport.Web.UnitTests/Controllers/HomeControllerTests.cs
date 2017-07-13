using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Shouldly;
using System.IO;
using TimeCare.WorkSchedule;
using WorkScheduleExport.Web.Controllers;
using WorkScheduleExport.Web.Infrastructure;
using WorkScheduleExport.Web.Infrastructure.Delivery;
using WorkScheduleExport.Web.Infrastructure.Export;
using WorkScheduleExport.Web.Models;
using WorkScheduleExport.Web.UnitTests.Helpers;
using Xunit;

namespace WorkScheduleExport.Web.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        private HomeController controller;

        public HomeControllerTests()
        {
            IWorkScheduleReaderFactory workScheduleReaderFactory = Substitute.For<IWorkScheduleReaderFactory>();
            IWorkScheduleExporter workScheduleExporter = Substitute.For<IWorkScheduleExporter>();
            IWorkScheduleDeliveryService workScheduleDeliveryService = Substitute.For<IWorkScheduleDeliveryService>();

            controller = new HomeController(workScheduleReaderFactory, workScheduleExporter, workScheduleDeliveryService);
        }

        [Fact]
        public void ReturnsDefaultViewOnGet()
        {
            IActionResult result = controller.Index();

            result.ShouldNotBeNull();
            result.ShouldBeOfType<ViewResult>();
        }

        [Fact]
        public void ReturnsBadRequestWhenModelIsInvalid()
        {
            controller.ModelState.AddModelError("Schema file missing", "Schema file is missing");

            IActionResult result = controller.Index(new WorkScheduleExportViewModel());

            result.ShouldNotBeNull();
            result.ShouldBeOfType<ViewResult>();
        }

        [Fact]
        public void ReturnsICalendarFileWhenProvidingValidSchemaFile()
        {
            IWorkScheduleReader workScheduleReader = Substitute.For<IWorkScheduleReader>();
            workScheduleReader.Read().Returns(new WorkSchedule());

            IWorkScheduleReaderFactory workScheduleReaderFactory = Substitute.For<IWorkScheduleReaderFactory>();
            workScheduleReaderFactory.Create(Arg.Any<Stream>()).Returns(workScheduleReader);

            IWorkScheduleExporter workScheduleExporter = Substitute.For<IWorkScheduleExporter>();
            workScheduleExporter.Export(Arg.Any<WorkSchedule>()).Returns(new byte[0]);

            IWorkScheduleDeliveryService workScheduleDeliveryService = Substitute.For<IWorkScheduleDeliveryService>();

            WorkScheduleExportViewModel model = new WorkScheduleExportViewModel
            {
                SchemaFile = new DummyFormFile()
            };

            controller = new HomeController(workScheduleReaderFactory, workScheduleExporter, workScheduleDeliveryService);

            IActionResult result = controller.Index(model);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<FileContentResult>();
        }

        [Fact]
        public void SendsSchemaToEmailWhenProvided()
        {
            IWorkScheduleReader workScheduleReader = Substitute.For<IWorkScheduleReader>();
            workScheduleReader.Read().Returns(new WorkSchedule());

            IWorkScheduleReaderFactory workScheduleReaderFactory = Substitute.For<IWorkScheduleReaderFactory>();
            workScheduleReaderFactory.Create(Arg.Any<Stream>()).Returns(workScheduleReader);

            IWorkScheduleExporter workScheduleExporter = Substitute.For<IWorkScheduleExporter>();
            workScheduleExporter.Export(Arg.Any<WorkSchedule>()).Returns(new byte[0]);

            IWorkScheduleDeliveryService workScheduleDeliveryService = Substitute.For<IWorkScheduleDeliveryService>();

            WorkScheduleExportViewModel model = new WorkScheduleExportViewModel
            {
                Email = "some-email@localhost",
                SchemaFile = new DummyFormFile()
            };

            controller = new HomeController(workScheduleReaderFactory, workScheduleExporter, workScheduleDeliveryService);

            IActionResult result = controller.Index(model);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<ViewResult>();
            workScheduleDeliveryService.ReceivedWithAnyArgs(1).Deliver(null, null, null);
        }
    }
}
