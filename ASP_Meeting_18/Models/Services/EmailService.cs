﻿using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Configuration;

namespace ASP_Meeting_18.Models.Services
{
    public interface IEmailService
    {
        void Send(string from, string to, string subject, string html);
    }

    public class EmailService : IEmailService
    {
        public Microsoft.Extensions.Configuration.ConfigurationManager configuration { get; set; }
        public EmailService(Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            this.configuration = configuration;
        }

        public void Send(string from, string to, string subject, string html)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            string str = configuration.GetSection("Smtplog").Value.ToString();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(configuration.GetSection("Smtplog").Value.ToString(), configuration.GetSection("Smtppass").Value.ToString());
            smtp.Send(email);
            smtp.Disconnect(true);

        }
    }
}
