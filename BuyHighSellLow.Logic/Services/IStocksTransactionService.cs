using BuyHighSellLow.Logic.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyHighSellLow.Logic.Services
{
    public interface IStocksTransactionService
    {
        Task BuyStocks(StocksTransactionRequest request);
        Task SellStocks(StocksTransactionRequest request);
        Task<decimal> CalculateTotalOrderPrice(List<StockOrder> stockOrders);
    }
}