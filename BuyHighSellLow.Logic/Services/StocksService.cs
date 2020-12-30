﻿using BuyHighSellLow.DataAccess;
using BuyHighSellLow.Logic.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using BuyHighSellLow.DataAccess.Models;

namespace BuyHighSellLow.Logic.Services
{
    public class StocksService : IStocksService
    {
        private readonly BHSLContext _context;

        public StocksService(BHSLContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task AddStocksToAccount(List<StockOrder> orders, string username)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => string.Equals(x.UserName, username, StringComparison.OrdinalIgnoreCase));
                var userHoldings = _context.UserHoldings.Where(x => x.User == user).ToList();
                if (userHoldings.Count < 1)
                {
                    var newUserHoldings = new List<UserHolding>();
                    foreach (var order in orders)
                    {
                        var stock = await GetStockFromDb(order.Ticker).ConfigureAwait(false);
                        newUserHoldings.Add(new UserHolding { Amount = order.Amount, Stock = stock, AveragePrice = stock.Price, LastChanged = DateTime.Now, User = user });
                    }

                    userHoldings.AddRange(newUserHoldings);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task RemoveStocks(List<StockOrder> orders, string username)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => string.Equals(x.UserName, username, StringComparison.OrdinalIgnoreCase));
                var userHoldings = _context.UserHoldings.Where(x => x.User == user).ToList();
                if (userHoldings.Count < 1) throw new Exception("User has no stocks to remove");

                foreach (var order in orders)
                {
                    var holding = userHoldings.Find(x => string.Equals(x.Stock.Ticker, order.Ticker, StringComparison.OrdinalIgnoreCase));

                    if (holding == null) throw new Exception($"User does not hold stock: {order.Ticker}. Failed to sell");
                    if(order.Amount > holding.Amount) throw new Exception($"Sell order is for more stocks than user owns. Failed to sell: {order.Ticker}.");

                    if (holding.Amount - order.Amount == 0) userHoldings.Remove(holding);
                    else holding.Amount -= order.Amount;
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<Stock> GetStockFromDb(string ticker)
        {
            try
            {
                var stock = _context.Stocks.FirstOrDefault(x => string.Equals(x.Ticker, ticker, StringComparison.OrdinalIgnoreCase));
                if (stock != null) return stock;

                var newStock = await AddStockToDb(ticker).ConfigureAwait(false);
                await _context.Stocks.AddAsync(newStock).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                return newStock;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<Stock> AddStockToDb(string ticker)
        {
            try
            {
                var stock = _context.Stocks.Find(ticker);

                if (stock != null) return stock;

                stock = await GetStocksFromApi(new string[] { ticker }).ConfigureAwait(false);
                await _context.Stocks.AddAsync(stock).ConfigureAwait(false);

                return stock;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<Stock> GetStocksFromApi(string[] ticker)
        {
            throw new NotImplementedException();
        }
    }
}
