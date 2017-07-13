using System.IO;

namespace WorkScheduleExport.Web.UnitTests.Helpers
{
    public class SchemaSourceHelper
    {
        public static Stream SchemaSource()
        {
            return File.OpenRead("SchemaFile.html");
        }
    }
}
