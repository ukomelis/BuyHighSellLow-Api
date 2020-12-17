using System.ComponentModel.DataAnnotations;

namespace BuyHighSellLow.DataAccess.Models
{
    public class TransactionType
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
    }
}