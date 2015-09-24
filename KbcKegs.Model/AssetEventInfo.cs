using KbcKegs.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Model
{
    public enum AssetEventType
    {
        Added,
        Retired,
        Delivered,
        Collected,
    }

    public class AssetEventInfo : BaseEntity
    {
        [ForeignKey("Asset")]
        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }

        public AssetEventType EventType { get; set; }
        public int EventId { get; set; }
    }
}
