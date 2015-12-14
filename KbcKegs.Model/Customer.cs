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
    public class Customer : BaseEntity
    {
        [Index]
        [MaxLength(128)]
        public string SourceId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<CollectionEvent> Collections { get; set; }
    }
}
