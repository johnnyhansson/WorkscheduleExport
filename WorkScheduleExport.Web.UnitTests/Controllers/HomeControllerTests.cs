using Microsoft.AspNetCore.Mvc;
using Shouldly;
using WorkScheduleExport.Web.Controllers;
using Xunit;

namespace WorkScheduleExport.Web.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        private HomeController controller;

        public HomeControllerTests()
        {
            controller = new HomeController();
        }

        [Fact]
        public void ReturnsDefaultViewOnGet()
        {
            IActionResult result = controller.Index();

            result.ShouldNotBeNull();
            result.ShouldBeOfType<ViewResult>();
        }
    }
}
