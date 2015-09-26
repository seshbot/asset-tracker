using KbcKegs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Models
{
    public class WarehouseViewModel
    {
        public WarehouseViewModel()
        {
            RecentDeliveries = new List<DeliveryEvent>();
            RecentCollections = new List<CollectionEvent>();
            Customers = new List<Customer>();
            Orders = new List<Order>();
            AssetsWithCustomers = new List<Asset>();
            AssetsAvailable = new List<Asset>();
        }

        public IEnumerable<DeliveryEvent> RecentDeliveries { get; set; }
        public IEnumerable<CollectionEvent> RecentCollections { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Asset> AssetsWithCustomers { get; set; }
        public IEnumerable<Asset> AssetsAvailable { get; set; }
    }
}
