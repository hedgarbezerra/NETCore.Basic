﻿using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using NETCore.Basic.Domain.Models;
using NETCore.Basic.Util.Configuration;
using NETCore.Basic.Util.Crypto;
using System;
using System.Collections.Generic;

namespace NETCore.Basic.Tests.Services.External
{
    public interface IMailing
    {
        void AdicionarAnexo(TransaferenceFile file);
        void Notify(List<string> receiverAddresses, string title, string message = "");
        void Notify(string receiverAddress, string title, string message = "");
    }
    public class Mailing : IMailing
    {
        private readonly MimeKit.Text.TextFormat _defaultFormat;
        private MimeMessage _message;
        private BodyBuilder _messageBodyBuilder;
        private readonly EmailSettings _settings;

        public Mailing(IConfiguration config, IEncryption encryption, MimeKit.Text.TextFormat format = MimeKit.Text.TextFormat.Text)
        {
            _settings = new EmailSettings(config, encryption);
            _message = NewMessage("Title");
            _messageBodyBuilder = new BodyBuilder();
            _defaultFormat = format;
        }

        public void Notify(List<string> receiverAddresses, string title, string message = "")
        {
            receiverAddresses.ForEach(x => _message.To.Add(new MailboxAddress(x)));

            if (_defaultFormat == MimeKit.Text.TextFormat.Text)
                _messageBodyBuilder.TextBody = message;
            else
                _messageBodyBuilder.HtmlBody = message;

            _message.Subject = title;
            _message.Body = _messageBodyBuilder.ToMessageBody();

            Enviarmessage(_message);
        }

        public void Notify(string receiverAddress, string title, string message = "")
        {
            _message.To.Add(new MailboxAddress(receiverAddress));

            if (_defaultFormat == MimeKit.Text.TextFormat.Text)
                _messageBodyBuilder.TextBody = message;
            else
                _messageBodyBuilder.HtmlBody = message;

            _message.Subject = title;
            _message.Body = _messageBodyBuilder.ToMessageBody();

            Enviarmessage(_message);
        }

        private MimeMessage NewMessage(string titleMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(titleMessage, _settings.SMTPEmail));

            return message;
        }
        public void AdicionarAnexo(TransaferenceFile file)
        {
            var multipart = new Multipart("mixed");

            var attachment = new MimePart(file.Type)
            {
                Content = new MimeContent(file.Content),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = file.Name
            };

            multipart.Add(attachment);

            _messageBodyBuilder.Attachments.Add(multipart);
        }
        private void Enviarmessage(MimeMessage message)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    client.CheckCertificateRevocation = false;
                    client.Connect(_settings.SMTPHostname, _settings.SMTPPort, MailKit.Security.SecureSocketOptions.StartTls);

                    client.Authenticate(_settings.SMTPEmail, _settings.SMTPPassword);

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong.", ex);
            }
        }
    }
}
