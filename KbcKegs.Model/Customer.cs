using KbcKegs.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Model
{
    public class Customer : BaseEntity
    {
        public string SourceId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<CollectionEvent> Collections { get; set; }
    }
}
