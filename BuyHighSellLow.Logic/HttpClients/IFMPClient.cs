using BuyHighSellLow.DataAccess.Models;
using BuyHighSellLow.Logic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyHighSellLow.Logic.HttpClients
{
    public interface IFMPClient
    {
        Task<List<Stock>> GetStocksPrices(string[] tickers);
    }
}