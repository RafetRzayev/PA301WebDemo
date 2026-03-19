using System;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Pa301Fiorelle.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public SmtpEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpSection = _config.GetSection("Smtp");
            var host = smtpSection.GetValue<string>("Host");
            var port = smtpSection.GetValue<int>("Port");
            var user = smtpSection.GetValue<string>("User");
            var pass = smtpSection.GetValue<string>("Pass");
            var from = smtpSection.GetValue<string>("From") ?? user;

            if (string.IsNullOrWhiteSpace(host))
                throw new InvalidOperationException("SMTP host is not configured. Check the 'Smtp:Host' setting in appsettings.");

            // optional SSL setting (default true)
            var enableSsl = smtpSection.GetValue<bool?>("EnableSsl") ?? true;

            // ensure host resolves early so we can give a clearer error
            try
            {
                Dns.GetHostEntry(host);
            }
            catch (SocketException ex)
            {
                throw new InvalidOperationException($"Cannot resolve SMTP host '{host}'. DNS lookup failed: {ex.Message}", ex);
            }

            try
            {
                using var client = new SmtpClient(host, port > 0 ? port : 25)
                {
                    Credentials = new NetworkCredential(user, pass),
                    EnableSsl = enableSsl
                };

                var mail = new MailMessage(from, email, subject, htmlMessage)
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(mail);
            }
            catch (SmtpException ex)
            {
                throw new InvalidOperationException($"Failed to send email via SMTP host '{host}' on port {port}. {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unexpected error when sending email via SMTP host '{host}': {ex.Message}", ex);
            }
        }
    }
}
