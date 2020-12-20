using System;
using Microsoft.Extensions.Configuration;

namespace BuyHighSellLow.Logic.Models.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IConfiguration _configuration;

        public ConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GetJwtSecret() => _configuration["Jwt:JwtSecret"];
        public string GetJwtIssuer() => _configuration["Jwt:JwtIssuer"];
        public string GetJwtAudience() => _configuration["Jwt:JwtAudience"];
        public string GetIEXCloudApiToken() => _configuration["APIInfos:IEXCloudToken"];
        public string GetIEXCloudApiUrl() => _configuration["APIInfos:IEXCloudApiUrl"];
        public string GetFinnhubApiToken() => _configuration["APIInfos:FinnhubToken"];
        public string GetFinnhubApiUrl() => _configuration["APIInfos:FinnhubApiUrl"];
        public object GetFMPApiUrl() => _configuration["APIInfos:FMPApiUrl"];
        public object GetFMPApiToken() => _configuration["APIInfos:FMPToken"];
    }
}