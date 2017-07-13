using System;
using System.IO;
using MimeKit;
using MailKit.Net.Smtp;
using TimeCare.WorkSchedule;

namespace WorkScheduleExport.Web.Infrastructure.Delivery
{
    public class EmailDelivery : IWorkScheduleDeliveryService
    {
        private readonly EmailConfiguration configuration;

        public EmailDelivery(EmailConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this.configuration = configuration;
        }

        public void Deliver(WorkSchedule workSchedule, byte[] exportedWorkSchedule, WorkScheduleDeliveryOptions options)
        {
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = $"Här kommer ditt arbetsschema för perioden {workSchedule}.";
            bodyBuilder.Attachments.Add("Arbetsschema.ics", new MemoryStream(exportedWorkSchedule));

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(configuration.From));
            message.To.Add(new MailboxAddress(options.Target));
            message.Subject = $"Ditt arbetsschema för perioden {workSchedule}";
            message.Body = bodyBuilder.ToMessageBody();

            using (SmtpClient client = new SmtpClient())
            {
                client.Connect(configuration.Host, configuration.Port, configuration.UseSsl);

                if (configuration.ContainCredentials)
                {
                    client.Authenticate(configuration.Username, configuration.Password);
                }

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}