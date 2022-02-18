using Microsoft.Extensions.Configuration;
using NETCore.Basic.Util.Crypto;

namespace NETCore.Basic.Util.Configuration
{
    public class AzureSettings : BaseSettings
    {
        public AzureSettings(IConfiguration config)
            : base(config)
        {
        }
        public AzureSettings(IConfiguration config, IEncryption encryption)
          : base(config, encryption)
        {
        }
        public string StorageKey { get => GetConfiguration("AZR-STORAGE-KEY", "AZR_STORAGE_KEY"); }
        public string StorageContainer { get => GetConfiguration("AZR-STORAGE-CONTAINER", "AZR_STORAGE_CONTAINER", true); }
        public string StorageConnectionString { get => GetConfiguration("AZR-STORAGE-CONNSTR", "AZR_STORAGE_CONNSTR"); }

        public string KeyVaultClientId { get => GetConfiguration("AZR_KV_APP_ID"); }
        public string KeyVaultURI { get => GetConfiguration("AZR_KV_URI"); }
        public string KeyVaultKey { get => GetConfiguration("AZR_KV_KEY"); }
    }
}
