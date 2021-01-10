using BuyHighSellLow.DataAccess.Models;
using BuyHighSellLow.DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BuyHighSellLow.DataAccess
{
    public class BHSLContext : IdentityDbContext<BHSLUser>
    {
        public BHSLContext(DbContextOptions<BHSLContext> options) : base(options)
        {

        }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<UserHolding> UserHoldings { get; set; }
    }
}