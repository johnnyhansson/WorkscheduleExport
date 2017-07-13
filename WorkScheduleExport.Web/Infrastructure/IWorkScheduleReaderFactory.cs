using System.IO;
using TimeCare.WorkSchedule;

namespace WorkScheduleExport.Web.Infrastructure
{
    public interface IWorkScheduleReaderFactory
    {
        IWorkScheduleReader Create(Stream stream);
    }
}
