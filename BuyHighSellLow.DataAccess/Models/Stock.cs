using System;
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
        //[Required]
        //public Currency Currency { get; set; };
        public decimal Price { get; set; }
        public DateTime PriceLastUpdated { get; set; }
    }
}