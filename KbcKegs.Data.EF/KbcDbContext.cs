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
        public DbSet<AssetType> AssetTypes { get; set; }
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

            var assetTypes = new Dictionary<string, AssetType>
            {
                { "S10", new AssetType { Description = "Schaeffer 10L Keg", AssetPrefix = "S10" } },
                { "S15", new AssetType { Description = "Schaeffer 15L Keg", AssetPrefix = "S15" } },
                { "S20", new AssetType { Description = "Schaeffer 20L Keg", AssetPrefix = "S20" } },
                { "U10", new AssetType { Description = "Used 10L Keg", AssetPrefix = "U10" } },
                { "U20", new AssetType { Description = "Used 20L Keg", AssetPrefix = "U20" } },
            };

            context.AssetTypes.AddRange(assetTypes.Values);

            context.SaveChanges();

            context.Assets.AddRange(new List<Asset>
            {
                new Asset { SerialNumber = "S200001", AssetType = assetTypes["S20"], State = AssetState.Available },
                new Asset { SerialNumber = "S200002", AssetType = assetTypes["S20"], State = AssetState.Available },
                new Asset { SerialNumber = "S200003", AssetType = assetTypes["S20"], State = AssetState.Available },
                new Asset { SerialNumber = "S200004", AssetType = assetTypes["S20"], State = AssetState.Available },
                new Asset { SerialNumber = "S200005", AssetType = assetTypes["S20"], State = AssetState.Available },
                new Asset { SerialNumber = "S200006", AssetType = assetTypes["S20"], State = AssetState.Available },
                new Asset { SerialNumber = "S200007", AssetType = assetTypes["S20"], State = AssetState.Available },
                new Asset { SerialNumber = "S200008", AssetType = assetTypes["S20"], State = AssetState.Available },
                new Asset { SerialNumber = "S200009", AssetType = assetTypes["S20"], State = AssetState.Available },
                new Asset { SerialNumber = "S200010", AssetType = assetTypes["S20"], State = AssetState.Available },
                new Asset { SerialNumber = "S200011", AssetType = assetTypes["S20"], State = AssetState.Available },
                new Asset { SerialNumber = "S200012", AssetType = assetTypes["S20"], State = AssetState.Available },
                new Asset { SerialNumber = "S200013", AssetType = assetTypes["S20"], State = AssetState.Available },
                new Asset { SerialNumber = "S200014", AssetType = assetTypes["S20"], State = AssetState.Available },
                new Asset { SerialNumber = "S150001", AssetType = assetTypes["S15"], State = AssetState.Available },
                new Asset { SerialNumber = "S150002", AssetType = assetTypes["S15"], State = AssetState.Available },
                new Asset { SerialNumber = "S150003", AssetType = assetTypes["S15"], State = AssetState.Available },
                new Asset { SerialNumber = "S150004", AssetType = assetTypes["S15"], State = AssetState.Available },
                new Asset { SerialNumber = "S150005", AssetType = assetTypes["S15"], State = AssetState.Available },
                new Asset { SerialNumber = "S100001", AssetType = assetTypes["S10"], State = AssetState.Available },
                new Asset { SerialNumber = "S100002", AssetType = assetTypes["S10"], State = AssetState.Available },
                new Asset { SerialNumber = "S100003", AssetType = assetTypes["S10"], State = AssetState.Available },
                new Asset { SerialNumber = "S100004", AssetType = assetTypes["S10"], State = AssetState.Available },
                new Asset { SerialNumber = "S100005", AssetType = assetTypes["S10"], State = AssetState.Available },
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
