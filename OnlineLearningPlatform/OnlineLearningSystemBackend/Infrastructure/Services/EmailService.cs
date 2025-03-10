using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public async Task sendEmailAsync(string email, string subject, string body)
        {
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            string fromEmail = _configuration["Smtp:From"]; // Use an environment variable for security
            string fromPassword = _configuration["Smtp:Password"]; // Use an environment variable for security

            try
            {
                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true; // Enable TLS encryption
                    client.Credentials = new NetworkCredential(fromEmail, fromPassword);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = false, // Set to true if you want to send HTML emails
                    };

                    mailMessage.To.Add(email);

                    // Send email asynchronously
                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                // Log the exception details for debugging
                throw;
            }
        }

    }

}
