using KbcKegs.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Description { get; set; }

        [Required]
        public string SerialNumber { get; set; }
        [Required]
        public AssetState State { get; set; }

        public virtual ICollection<AssetEventInfo> History { get; set; }
    }
}
