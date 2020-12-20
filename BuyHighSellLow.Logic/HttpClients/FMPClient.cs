using BuyHighSellLow.Logic.Models;
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

        public async Task<List<StockData>> GetStocksPrice(string[] tickers)
        {
            var stocksData = new List<StockData>();

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"{_configurationProvider.GetFMPApiUrl()}/quote/{string.Join(",", tickers)}?apikey={_configurationProvider.GetFMPApiToken()}");
                request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var result = await JsonSerializer.DeserializeAsync<JsonDocument>(responseStream);
                    var b = result.RootElement;
                    foreach (var item in b.EnumerateArray())
                    {
                        item.TryGetProperty("symbol", out var ticker);
                        item.TryGetProperty("price", out var price);
                        stocksData.Add(new StockData { Ticker = ticker.ToString(), Price = Convert.ToDecimal(price.ToString()) });
                    }                    
                }
                else
                {
                    throw new Exception("Request to get stock prices was not successful");
                }

                return stocksData;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get stock prices", ex);
            }
        }
    }
}