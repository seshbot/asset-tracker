using KbcKegs.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Model
{
    public class DeliveryEvent : BaseEvent
    {
        public virtual ICollection<OrderFulfillment> OrderFulfillments { get; set; }
    }

    public class OrderFulfillment : BaseEntity
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
    }
}
