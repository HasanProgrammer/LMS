using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Common;
using DataAccess;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Service.Web.Mail
{
    public class EmailSender : IMailSender
    {
        private readonly Config.Mail _Config;
        
        public EmailSender(IOptions<Config.Mail> Config)
        {
            _Config = Config.Value;
        }

        public async Task SendAsync(List<string> to, string subject, string data)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_Config.FromAddress, _Config.FromAddress));
                to.ForEach(UserMail => message.To.Add(new MailboxAddress(UserMail, UserMail)));
                message.Subject = subject;
                var builder     = new BodyBuilder { TextBody = data };
                message.Body    = builder.ToMessageBody();

                var client = new SmtpClient();
                await client.ConnectAsync(_Config.Server, _Config.Port, false);
                await client.AuthenticateAsync(_Config.UsernameMail, _Config.PasswordMail);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task SendFromHtmlAsync(List<string> to, string subject, string file)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_Config.FromAddress, _Config.FromAddress));
                to.ForEach(UserMail => message.To.Add(new MailboxAddress(UserMail, UserMail)));
                message.Subject  = subject;
                var builder      = new BodyBuilder { HtmlBody = await File.OpenText(file).ReadToEndAsync() };
                message.Body     = builder.ToMessageBody();

                var client = new SmtpClient();
                await client.ConnectAsync(_Config.Server, _Config.Port, false); 
                await client.AuthenticateAsync(_Config.UsernameMail, _Config.PasswordMail);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}