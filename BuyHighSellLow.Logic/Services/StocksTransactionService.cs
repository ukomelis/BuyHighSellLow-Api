using BuyHighSellLow.Logic.HttpClients;
using BuyHighSellLow.Logic.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuyHighSellLow.Logic.Services
{
    public class StocksTransactionService : IStocksTransactionService
    {
        private readonly IFMPClient _FMPClient;
        private readonly IUserService _userService;

        public StocksTransactionService(IFMPClient FMPClient, IUserService userService)
        {
            _FMPClient = FMPClient;
            _userService = userService;
        }

        public async Task BuyStocks(StocksTransactionRequest request)
        {
            try
            {
                var totalPrice = await CalculateTotalOrderPrice(request.Orders).ConfigureAwait(false);
                var userBalance = await _userService.GetUserBalance(request.Username).ConfigureAwait(false);

                if (userBalance < totalPrice) throw new Exception("Not enough balance");

                //Add stocks to acc and take payment
                await _userService.AddStocksToAccount(request.Orders, request.Username, totalPrice).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SellStocks(StocksTransactionRequest request)
        {
            try
            {
                var tickers = new List<string>();
                request.Orders.ForEach(x => tickers.Add(x.Ticker));

                var totalPrice = await CalculateTotalOrderPrice(request.Orders).ConfigureAwait(false);

                //Add stocks to acc and take payment
                await _userService.RemoveStocksFromAccount(request.Orders, request.Username, totalPrice).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<decimal> CalculateTotalOrderPrice(List<StockOrder> stockOrders)
        {
            try
            {
                var tickers = new List<string>();
                stockOrders.ForEach(x => tickers.Add(x.Ticker));

                var stocksData = await _FMPClient.GetStockData(tickers.ToArray()).ConfigureAwait(false);

                decimal totalPrice = 0;
                stocksData.ForEach(x => {
                    var order = stockOrders.Find(o => o.Ticker.Equals(x.Ticker, StringComparison.OrdinalIgnoreCase));
                    totalPrice += x.Price * order.Amount;
                });

                return totalPrice;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to calculate total price: {ex}");
                throw;
            }
        }
    }
}
