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

            context.Assets.AddRange(new List<Asset>
            {
                new Asset { SerialNumber = "S200001", Description = "20L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S200002", Description = "20L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S200003", Description = "20L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S200004", Description = "20L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S200005", Description = "20L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S200006", Description = "20L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S200007", Description = "20L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S200008", Description = "20L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S200009", Description = "20L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S200010", Description = "20L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S200011", Description = "20L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S200012", Description = "20L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S200013", Description = "20L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S200014", Description = "20L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S150001", Description = "15L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S150002", Description = "15L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S150003", Description = "15L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S150004", Description = "15L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S150005", Description = "15L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S100001", Description = "10L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S100002", Description = "10L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S100003", Description = "10L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S100004", Description = "10L Keg", State = AssetState.Available },
                new Asset { SerialNumber = "S100005", Description = "10L Keg", State = AssetState.Available },
            });

            context.SaveChanges();

            var customers = new Dictionary<string, Customer>
            {
                { "Cechner's Brews", new Customer { SourceId = "CUST001", Name = "Cechner's Brews" } },
                { "Speedo's Brews", new Customer { SourceId = "CUST002", Name = "Speedo's Brews" } },
            };

            context.Customers.AddRange(customers.Values);

            context.SaveChanges();

            context.Orders.AddRange(new List<Order>
            {
                new Order { Customer = customers["Cechner's Brews"], SourceId = "ORD001" },
                new Order { Customer = customers["Cechner's Brews"], SourceId = "ORD002" },
                new Order { Customer = customers["Cechner's Brews"], SourceId = "ORD003" },
                new Order { Customer = customers["Speedo's Brews"], SourceId = "ORD004" },
                new Order { Customer = customers["Speedo's Brews"], SourceId = "ORD005" },
                new Order { Customer = customers["Speedo's Brews"], SourceId = "ORD006" },
                new Order { Customer = customers["Speedo's Brews"], SourceId = "ORD007" },
                new Order { Customer = customers["Speedo's Brews"], SourceId = "ORD008" },
            });

            context.SaveChanges();
        }
    }
}
