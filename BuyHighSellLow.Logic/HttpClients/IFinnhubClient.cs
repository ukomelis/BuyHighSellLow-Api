using BuyHighSellLow.Logic.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyHighSellLow.Logic.HttpClients
{
    public interface IFinnhubClient
    {
        Task<IEnumerable<FinnhubStockSymbolsResponse>> GetMarketStocks(string exchange, string marketIdentifierCode = null);
    }
}