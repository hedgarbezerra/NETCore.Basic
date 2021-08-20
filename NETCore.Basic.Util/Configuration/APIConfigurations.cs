using Microsoft.Extensions.Configuration;

namespace NETCore.Basic.Util.Configuration
{

    public interface IAPIConfigurations
    {
        public string ConnectionString { get; }
        public string CryptoKey { get; }
        public string LoggingTable { get; }
    }
    public class APIConfigurations : IAPIConfigurations
    {
        public IConfiguration _configuration { get; }

        public string ConnectionString => _configuration.GetConnectionString("MainString");

        public string CryptoKey => _configuration.GetSection("ConfiguratonKeys")["Encryption"] ?? string.Empty;

        public string LoggingTable => _configuration.GetSection("Logging")["Table"] ?? string.Empty;

        public APIConfigurations(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
