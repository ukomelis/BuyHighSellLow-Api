using BuyHighSellLow.Logic.Models.Configuration;
using BuyHighSellLow.Logic.Models.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuyHighSellLow.Logic.HttpClients
{
    public class FinnhubClient : IFinnhubClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfigurationProvider _configurationProvider;

        public FinnhubClient(IHttpClientFactory clientFactory, IConfigurationProvider configurationProvider)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        }

        public async Task<IEnumerable<FinnhubStockSymbolsResponse>> GetMarketStocks(string exchange, string marketIdentifierCode = null)
        {
            try
            {
                var reqUrl = $"{ _configurationProvider.GetFinnhubApiUrl() }/stock/symbol?exchange={exchange}";
                if (marketIdentifierCode != null) reqUrl += reqUrl + $"&mic={marketIdentifierCode}";

                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"{reqUrl}?apikey={_configurationProvider.GetFinnhubApiToken()}");
                //request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request).ConfigureAwait(false);

                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var result = await JsonSerializer.DeserializeAsync<List<FinnhubStockSymbolsResponse>>(responseStream).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get ", ex);
            }
        }
    }
}