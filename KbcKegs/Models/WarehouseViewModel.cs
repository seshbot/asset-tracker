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
        }

        public IEnumerable<DeliveryEvent> RecentDeliveries { get; set; }
        public IEnumerable<CollectionEvent> RecentCollections { get; set; }
    }
}
