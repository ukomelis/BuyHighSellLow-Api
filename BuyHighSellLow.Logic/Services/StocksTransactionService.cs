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
                var tickers = new List<string>();
                request.Orders.ForEach(x => tickers.Add(x.Ticker));

                var totalPrice = await CalculateTotalOrderPrice(tickers).ConfigureAwait(false);
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

                var totalPrice = await CalculateTotalOrderPrice(tickers).ConfigureAwait(false);

                //Add stocks to acc and take payment
                await _userService.RemoveStocksFromAccount(request.Orders, request.Username, totalPrice).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<decimal> CalculateTotalOrderPrice(List<string> tickers)
        {
            var stocksData = await _FMPClient.GetStocksPrice(tickers.ToArray()).ConfigureAwait(false);

            decimal totalPrice = 0;
            stocksData.ForEach(x => totalPrice += x.Price);

            return totalPrice;
        }
    }
}
