using KbcKegs.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Model
{
    public class AssetType : BaseEntity
    {
        [Required]
        [MaxLength(128)]
        [Index]
        public string Description { get; set; }

        [DisplayName("Asset Prefix")]
        public string AssetPrefix { get; set; }

        public virtual ICollection<Asset> Assets { get; set; }
    }
}
