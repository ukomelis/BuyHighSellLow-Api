using BuyHighSellLow.DataAccess;
using BuyHighSellLow.DataAccess.Models;
using BuyHighSellLow.Logic.HttpClients;
using BuyHighSellLow.Logic.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace BuyHighSellLow.Logic.Jobs
{
    public class StocksDataJobs : IStocksDataJobs
    {
        private readonly IFMPClient _fMPClient;
        private readonly IStocksService _stocksService;
        private readonly IFinnhubClient _finnhubClient;
        private readonly BHSLContext _context;

        public StocksDataJobs(IFMPClient fMPClient, IFinnhubClient finnhubClient, BHSLContext context, IStocksService stocksService)
        {
            _fMPClient = fMPClient ?? throw new ArgumentNullException(nameof(fMPClient));
            _stocksService = stocksService ?? throw new ArgumentNullException(nameof(stocksService));
            _finnhubClient = finnhubClient ?? throw new ArgumentNullException(nameof(finnhubClient));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAllStocksToDatabase()
        {
            try
            {
                var tickers = _stocksService.GetAllAvailableTickersFromDb();
                var stocks = await _fMPClient.GetStocksPrices(tickers).ConfigureAwait(false);

                await _stocksService.AddStocksToDatabase(stocks).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add all stocks to database!", ex);
            }
        }

        public async Task CheckAndAddNewTickersToDb(string exchange/* string marketIdentificerCode = null */)
        {
            try
            {
                var stocks = await _finnhubClient.GetMarketStocks(exchange).ConfigureAwait(false);
                var newStocks = new List<Stock>();

                foreach (var stock in stocks)
                {
                    var dbStock = _context.Stocks.FirstOrDefault(x => x.Ticker.Equals(stock.symbol, StringComparison.OrdinalIgnoreCase));
                    if (dbStock == null){
                        var currency = _context.Currencies.FirstOrDefault(x => x.Name.Equals(stock.symbol, StringComparison.OrdinalIgnoreCase)) ?? _context.Currencies.Add(new Currency { Name = stock.currency }).Entity;

                        newStocks.Add(new Stock
                        {
                            Name = stock.description,
                            Ticker = stock.symbol,
                            Currency = currency,
                            MIC = stock.mic
                        });
                    }
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to check for new tickers", ex);
            }
        }
    }
}
