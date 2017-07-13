namespace WorkScheduleExport.Web.Infrastructure.Delivery
{
    public class EmailConfiguration
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public bool UseSsl { get; set; }

        public string From { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool ContainCredentials => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
    }
}
