using System.Collections.Generic;

namespace BuyHighSellLow.Logic.Models.Requests
{
    public class StocksTransactionRequest
    {
        public List<StockTransactionData> Transactions { get; set; }        
        public string UserEmail { get; set; }
        public int TransactionType { get; set; }
    }
}