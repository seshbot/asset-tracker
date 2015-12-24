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
    public enum AssetState
    {
        Available,
        WithCustomer,
        NeedsCleaning,
        Retired,
    }

    public class Asset : BaseEntity
    {
        public Asset()
        {
            History = new List<AssetEventInfo>();
        }

        public string Description { get; set; }

        [Required]
        [MaxLength(128)]
        [Index]
        public string SerialNumber { get; set; }
        [Required]
        public AssetState State { get; set; }

        [ForeignKey("WithCustomer")]
        public int? WithCustomerId { get; set; }
        public Customer WithCustomer { get; set; }

        public virtual ICollection<AssetEventInfo> History { get; set; }
    }
}
