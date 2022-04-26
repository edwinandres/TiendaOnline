using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TiendaOnline.Utilidades
{
    public class EmailSender :IEmailSender
    {
        public string SenderGridSecret { get; set; }

        public EmailSender(IConfiguration _config)
        {
            SenderGridSecret = _config.GetValue<string>("SendGrid:SecretKey");
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(SenderGridSecret);
            var from = new EmailAddress("edwin.roman@tiglobal.com.co");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            
            return client.SendEmailAsync(msg);
        }
    }
}
