using BuyHighSellLow.Logic.HttpClients;
using BuyHighSellLow.Logic.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuyHighSellLow.Logic.Services
{
    public class StocksTransactionsService : IStocksTransactionsService
    {
        private readonly IFMPClient _FMPClient;

        public StocksTransactionsService(IFMPClient FMPClient)
        {
            _FMPClient = FMPClient;
        }
        public async Task BuyStocks(StocksTransactionRequest request)
        {
            var tickers = new List<string>();
            request.Transactions.ForEach(x => tickers.Add(x.Ticker));

            await _FMPClient.GetStockPrices(tickers.ToArray()).ConfigureAwait(false);
        }

        public Task SellStocks(StocksTransactionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
