namespace BuyHighSellLow.Logic.Models.Configuration
{
    public interface IConfigurationProvider
    {
        string GetIEXCloudApiToken();
        string GetIEXCloudApiUrl();
        string GetFinnhubApiToken();
        string GetFinnhubApiUrl();
        string GetJwtSecret();
        string GetJwtIssuer();
        string GetJwtAudience();
        object GetFMPApiUrl();
        object GetFMPApiToken();
    }
}