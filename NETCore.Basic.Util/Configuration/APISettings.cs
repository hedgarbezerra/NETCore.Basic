using Microsoft.Extensions.Configuration;
using NETCore.Basic.Util.Crypto;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Util.Configuration
{
    public class APISettings : BaseSettings
    {
        public APISettings(IConfiguration config)
               : base(config)
        {
        }

        public string ConnectionString { get => GetConfiguration("DEFAULTCONNECTIONSTR", "ConnectionStrings:DEFAULT"); }
        public string EncryptionKey { get => GetConfiguration("ENCRYPTIONKEY", "ENCRYPTION_KEY"); }
        public string TokenKey { get => GetConfiguration("TOKEN-KEY", "TOKEN_KEY"); }
    }
}
