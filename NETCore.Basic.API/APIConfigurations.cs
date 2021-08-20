using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.Basic.API
{
    public interface IAPIConfigurations
    {
        public string ConnectionString { get;}
        public string CryptoKey { get; }
        public string HashingKey { get; }
    }
    public class APIConfigurations : IAPIConfigurations
    {
        public IConfiguration _configuration { get; }

        public string ConnectionString => _configuration.GetConnectionString("MainString");

        public string CryptoKey => _configuration.GetSection("ConfiguratonKeys").GetValue("Encryption", string.Empty);

        public string HashingKey => _configuration.GetSection("ConfiguratonKeys").GetValue("Hashing", string.Empty);

        public APIConfigurations(IConfiguration configuration)
        {
            _configuration = configuration;
        }


    }
}
