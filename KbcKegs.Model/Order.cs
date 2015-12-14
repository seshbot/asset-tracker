using KbcKegs.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Model
{
    public class Order : BaseEntity
    {
        [Index]
        [MaxLength(128)]
        public string SourceId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual ICollection<DeliveryEvent> Deliveries { get; set; }
    }
}
