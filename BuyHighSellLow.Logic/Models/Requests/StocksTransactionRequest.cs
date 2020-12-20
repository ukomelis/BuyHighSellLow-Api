using System.Collections.Generic;

namespace BuyHighSellLow.Logic.Models.Requests
{
    public class StocksTransactionRequest
    {
        public List<StockOrder> Orders { get; set; }        
        public string Username { get; set; }
    }
}