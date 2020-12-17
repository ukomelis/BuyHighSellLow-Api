using System.Threading.Tasks;

namespace BuyHighSellLow.Logic.HttpClients
{
    public interface IFMPClient
    {
        Task GetStockPrices(string[] tickers);
    }
}