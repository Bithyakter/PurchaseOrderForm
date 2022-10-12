using Microsoft.EntityFrameworkCore;

namespace PurchaseDetailTask.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Sell> Sells { get; set; }
        public DbSet<Details> Details { get; set; }
    } 
}