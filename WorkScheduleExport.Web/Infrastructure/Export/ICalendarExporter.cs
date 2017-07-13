using System.Text;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Serialization.iCalendar.Serializers;
using TimeCare.WorkSchedule;

namespace WorkScheduleExport.Web.Infrastructure.Export
{
    /// <summary>
    /// Exports a work schedule to ICalendar format.
    /// </summary>
    public class ICalendarExporter : IWorkScheduleExporter
    {
        public byte[] Export(WorkSchedule workSchedule)
        {
            var iCal = new Calendar
            {
                Method = "PUBLISH",
                Version = "2.0",
                
            };

            foreach (WorkShift workShift in workSchedule.WorkShifts)
            {
                var calendarEvent = iCal.Create<CalendarEvent>();
                calendarEvent.Class = "PUBLIC";
                calendarEvent.Summary = workShift.WorkCode;
                calendarEvent.Start = new CalDateTime(workShift.Start);
                calendarEvent.End = new CalDateTime(workShift.End);
                calendarEvent.Duration = workShift.Duration;
                calendarEvent.Description = workShift.Notes;
                calendarEvent.IsAllDay = false;
            }

            string calendarContent = new CalendarSerializer().SerializeToString(iCal);

            return Encoding.UTF8.GetBytes(calendarContent);
        }
    }
}
