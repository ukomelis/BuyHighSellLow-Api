using BuyHighSellLow.Logic.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuyHighSellLow.Logic.HttpClients
{
    public class FMPClient : IFMPClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfigurationProvider _configurationProvider;

        public FMPClient(IHttpClientFactory clientFactory, IConfigurationProvider configurationProvider)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        }

        public async Task GetStockPrices(string[] tickers)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            $"{_configurationProvider.GetFMPApiUrl()}/quote/{string.Join(",", tickers)}?apikey={_configurationProvider.GetFMPApiToken()}");
            request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<JsonTokenType>(responseStream);
            }
            else
            {

            }
        }
    }
}
