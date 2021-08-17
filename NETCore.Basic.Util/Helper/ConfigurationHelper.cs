using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Util.Helper
{
    public class ConfigurationHelper
    {
        private readonly IConfiguration _configuration;

        public ConfigurationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfigurationSection GetSettingsByKey (string item) => _configuration.GetSection(item);
    }
}
