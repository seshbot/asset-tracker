using System;
using System.Collections.Generic;
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

    public class Asset
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public string SerialNumber { get; set; }
        public AssetState State { get; set; }

        public virtual ICollection<AssetEvent> History { get; set; }
    }
}
