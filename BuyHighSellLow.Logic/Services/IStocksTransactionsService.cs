using BuyHighSellLow.Logic.Models.Requests;
using System.Threading.Tasks;

namespace BuyHighSellLow.Logic.Services
{
    public interface IStocksTransactionsService
    {
        Task BuyStocks(StocksTransactionRequest request);
        Task SellStocks(StocksTransactionRequest request);
    }
}
