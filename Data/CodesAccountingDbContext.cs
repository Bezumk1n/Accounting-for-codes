using CodesAccounting.Model;
using Microsoft.EntityFrameworkCore;

namespace CodesAccounting.Data
{
    public class CodesAccountingDbContext : DbContext
    {
        public DbSet<Codes> Codes { get; set; }

        public DbSet<Templates> Templates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = Database\DB.db");
        }
    }
}
