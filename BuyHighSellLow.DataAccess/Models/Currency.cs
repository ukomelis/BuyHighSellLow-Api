using System.ComponentModel.DataAnnotations;

namespace BuyHighSellLow.DataAccess.Models
{
    public class Currency
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string CurrencySymbol { get; set; }
        public string Description { get; set; }
    }
}