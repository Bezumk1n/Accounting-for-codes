using CodesAccounting.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;

namespace CodesAccounting
{
    public partial class App : Application
    {
        public App()
        {
            if (!File.Exists(@".\Database\DB.db"))
            {
                using (CodesAccountingDbContext context = new CodesAccountingDbContext())
                {
                    Directory.CreateDirectory("Database");

                    context.Database.Migrate();

                    DataSeed ds = new DataSeed();
                    ds.Seed(context);
                }
            }
        }
    }
}
