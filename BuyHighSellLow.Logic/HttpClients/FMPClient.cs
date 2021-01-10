using BuyHighSellLow.DataAccess.Models;
using BuyHighSellLow.Logic.Models;
using BuyHighSellLow.Logic.Models.Configuration;
using BuyHighSellLow.Logic.Models.Responses;
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

        public async Task<List<Stock>> GetStocksPrices(string[] tickers)
        {
            var stocksList = new List<Stock>();

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
                    $"{_configurationProvider.GetFMPApiUrl()}/quote/{string.Join(",", tickers)}?apikey={_configurationProvider.GetFMPApiToken()}");
                request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    var stocksResponse = await JsonSerializer.DeserializeAsync<List<FMPStockDataResponse>>(responseStream).ConfigureAwait(false);
                    foreach (var stock in stocksResponse)
                    {
                        stocksList.Add(new Stock
                        {
                            Ticker = stock.symbol,
                            Price = Convert.ToDecimal(stock.price),
                            PriceLastUpdated = DateTime.Now
                        });
                    }
                }
                else
                {
                    throw new Exception($"Request to get stock prices was unsuccessful: {response}");
                }

                return stocksList;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get stock prices", ex);
            }
        }
    }
}