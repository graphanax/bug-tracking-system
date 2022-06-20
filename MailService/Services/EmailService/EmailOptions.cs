namespace MailService.Services.EmailService
{
    public class EmailOptions
    {
        public static string SmtpHost { get; }
        public static int SmtpPort { get; }
        public static string MailAddressFrom { get; }
        public static string Username { get; }
        public static string Password { get; }
    }
}