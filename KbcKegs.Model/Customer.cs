using KbcKegs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Model
{
    public class Customer : BaseEntity
    {
        public string SourceId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
