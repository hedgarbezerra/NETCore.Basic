using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Util.Configuration
{
    public static class AzureSettings
    {
        public static string StorageKey { get; set; }
        public static string StorageContainer { get; set; }
        public static string StorageConnectionString { get; set; }

        public static string KeyVaultKey { get; set; }
    }
}
