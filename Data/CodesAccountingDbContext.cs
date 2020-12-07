using CodesAccounting.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
