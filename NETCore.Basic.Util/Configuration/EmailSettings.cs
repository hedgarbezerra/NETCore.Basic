using Microsoft.Extensions.Configuration;
using NETCore.Basic.Util.Crypto;
using System;

namespace NETCore.Basic.Util.Configuration
{
    public class EmailSettings : BaseSettings
    {
        public EmailSettings(IConfiguration config, IEncryption encryption)
            : base(config, encryption)
        {

        }
        public string SMTPEmail { get => GetConfiguration("SMTP-EMAIL-ADDRESS", "SMTP_EMAIL_ADDRESS"); }
        public string SMTPPassword { get => GetConfiguration("SMTP-EMAIL-PASSWORD", "SMTP_EMAIL_PASSWORD", true); }
        public int SMTPPort { get => Convert.ToInt32(GetConfiguration("SMTP-PORT", "SMTP_PORT") ?? "0"); }
        public string SMTPHostname { get => GetConfiguration("SMTP-HOSTNAME", "SMTP_HOSTNAME"); }
    }
}
