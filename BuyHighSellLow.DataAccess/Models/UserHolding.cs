using BuyHighSellLow.DataAccess.Models.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace BuyHighSellLow.DataAccess.Models
{
    public class UserHolding
    {
        public int Id { get; set; }
        [Required]
        public BHSLUser User { get; set; }
        [Required]
        public Stock Stock { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public decimal AveragePrice { get; set; }
        [Required]
        public decimal TotalAveragePrice { get; set; }
        [Required]
        public Currency Currency { get; set; }
        [Required]
        public DateTime LastChanged { get; set; }
    }
}