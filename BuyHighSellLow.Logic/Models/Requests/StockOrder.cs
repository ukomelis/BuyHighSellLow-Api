namespace BuyHighSellLow.Logic.Models.Requests
{
    public class StockOrder
    {
        public string Ticker { get; set; }
        public decimal Amount { get; set; }
    }
}