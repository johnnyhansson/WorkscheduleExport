using TimeCare.WorkSchedule;

namespace WorkScheduleExport.Web.Infrastructure.Delivery
{
    public interface IWorkScheduleDeliveryService
    {
        void Deliver(WorkSchedule workSchedule, byte[] exportedWorkSchedule, WorkScheduleDeliveryOptions options);
    }
}
