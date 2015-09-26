using KbcKegs.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Data
{
    public class KbcDbContext : DbContext
    {
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<DeliveryEvent> DeliveryEvents { get; set; }
        public DbSet<CollectionEvent> CollectionEvents { get; set; }
        public DbSet<CleaningEvent> CleaningEvents { get; set; }
    }

    public class KbcDbContextInitializer : DropCreateDatabaseIfModelChanges<KbcDbContext>
    {
        protected override void Seed(KbcDbContext context)
        {
            base.Seed(context);
        }
    }
}
