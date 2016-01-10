using KbcKegs.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Model
{
    public class CollectionEvent : BaseEvent
    {
        public CollectionEvent()
        {
            Assets = new List<Asset>();
        }

        public virtual ICollection<Asset> Assets { get; set; }
    }
}
