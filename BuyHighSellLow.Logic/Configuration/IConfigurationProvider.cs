namespace BuyHighSellLow.Logic.Models.Configuration
{
    public interface IConfigurationProvider
    {
        string GetIEXCloudApiKey();
        string GetIEXCloudApiUrl();
        string GetFinnhubApiToken();
        string GetFinnhubApiUrl();
        string GetJwtSecret();
        string GetJwtIssuer();
        string GetJwtAudience();
    }
}