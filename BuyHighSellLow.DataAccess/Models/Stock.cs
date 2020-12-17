using System.ComponentModel.DataAnnotations;

namespace BuyHighSellLow.DataAccess.Models
{
    public class Stock
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Ticker { get; set; }
    }
}