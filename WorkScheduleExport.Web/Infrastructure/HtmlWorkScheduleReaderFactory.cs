using System.IO;
using TimeCare.WorkSchedule;
using TimeCare.WorkSchedule.Html;

namespace WorkScheduleExport.Web.Infrastructure
{
    public class HtmlWorkScheduleReaderFactory : IWorkScheduleReaderFactory
    {
        public IWorkScheduleReader Create(Stream stream)
        {
            return new HtmlWorkScheduleReader(new HtmlDocument(stream));
        }
    }
}
