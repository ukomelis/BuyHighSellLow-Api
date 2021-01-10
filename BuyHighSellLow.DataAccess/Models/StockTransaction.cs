using BuyHighSellLow.DataAccess.Models.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyHighSellLow.DataAccess.Models
{
    public class StockTransaction
    {
        public int Id { get; set; }
        [Required]
        public TransactionType TransactionType { get; set; }
        [Required]
        public BHSLUser User { get; set; }
        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Amount { get; set; }
        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal SinglePrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalPrice { get; set; }
        [Required]
        public Stock Stock { get; set; }
        [Required]
        public Currency Currency { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
    }
}