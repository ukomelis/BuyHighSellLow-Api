using BuyHighSellLow.Logic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyHighSellLow.Logic.HttpClients
{
    public interface IFMPClient
    {
        Task<List<StockData>> GetStocksPrice(string[] tickers);
    }
}