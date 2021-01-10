using System.Threading.Tasks;

namespace BuyHighSellLow.Logic.Jobs
{
    public interface IStocksDataJobs
    {
        Task AddAllStocksToDatabase();
        Task CheckAndAddNewTickersToDb(string exchange);
    }
}
