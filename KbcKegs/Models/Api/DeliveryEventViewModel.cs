using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Models.Api
{
    public class DeliveryEventViewModel
    {
        public DeliveryEventViewModel()
        {
            OrderFulfillments = new List<OrderFulfillmentViewModel>();
            DateTime = DateTime.Now;
        }

        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public IEnumerable<OrderFulfillmentViewModel> OrderFulfillments { get; set; }
    }

    public class OrderFulfillmentViewModel
    {
        public OrderFulfillmentViewModel()
        {
            Assets = new List<AssetViewModel>();
        }

        public int? OrderId { get; set; }
        public string OrderSourceId { get; set; }
        public string OrderCustomerSourceId { get; set; }
        public IEnumerable<AssetViewModel> Assets { get; set; }
    }
}
