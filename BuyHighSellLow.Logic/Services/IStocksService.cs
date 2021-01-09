using BuyHighSellLow.Logic.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BuyHighSellLow.Logic.Services
{
    public interface IStocksService
    {
        Task AddStocksToAccount(List<StockOrder> orders, string username);
        Task RemoveStocks(List<StockOrder> orders, string username);
    }
}
