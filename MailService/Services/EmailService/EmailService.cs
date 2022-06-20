using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MailService.Services.ConsumerNotificationService;

namespace MailService.Services.EmailService
{
    public class EmailService
    {
        public static async Task SendNotificationOfAssigment(NotificationOfIssueAssignment options)
        {
            try
            {
                var message = new MailMessage
                {
                    From = new MailAddress(EmailOptions.MailAddressFrom),
                    To = { options.AssigneeEmail},
                    Subject = "Bug tracker | Assigment",
                    Body = $"You have been assigned to do issue #{options.IssueId}.",
                    IsBodyHtml = true
                };

                var client = new SmtpClient
                {
                    Host = EmailOptions.SmtpHost,
                    Port = EmailOptions.SmtpPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(EmailOptions.Username, EmailOptions.Password)
                };
                
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                Debug.Write($"Mail wasn't send: {ex.Message}");
            }
        }
    }
}