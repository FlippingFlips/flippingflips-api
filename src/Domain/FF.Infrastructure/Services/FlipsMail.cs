﻿using FF.Domain.Messaging;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FlippingFlips.Infrastructure.Service
{
    public class FlipsMail : IEmailSender
    {
        const string LOGO = "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";

        public FlipsMail(IOptions<FlipsMailMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public FlipsMailMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            if(Options.SendGridKey != null)
                return Execute(Options.SendGridKey, subject, message, email);

            throw new NullReferenceException("No SendGridKey found. Configure in appSettings.json");
        }

        public async Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.EmailFrom, Options.EmailFromName),
                Subject = subject,
                PlainTextContent = "",
                HtmlContent = "<img src='cid:logo'></img><br/>" +  message
            };
            msg.AddAttachment(@"logo.jpeg", LOGO, "image/jpeg", "inline", "logo");
            msg.SetFooterSetting(true, $"<br/> <br/><a href='{Options.EmailFooterUrl}'>{Options.SiteName}</a>", $"{Options.EmailFooterUrl}");
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            var result =  await client.SendEmailAsync(msg);
            if (!result.IsSuccessStatusCode)
                throw new Exception("Failed to send email, status code:" + result.StatusCode);
        }
    }
}