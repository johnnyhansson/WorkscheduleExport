using TimeCare.WorkSchedule;

namespace WorkScheduleExport.Web.Infrastructure.Export
{
    public interface IWorkScheduleExporter
    {
        byte[] Export(WorkSchedule workSchedule);
    }
}
