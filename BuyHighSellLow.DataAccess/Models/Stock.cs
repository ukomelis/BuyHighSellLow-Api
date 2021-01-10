using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyHighSellLow.DataAccess.Models
{
    public class Stock
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Ticker { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }
        public DateTime PriceLastUpdated { get; set; }
        [Required]
        public Currency Currency { get; set; }
        public string MIC { get; set; }
    }
}