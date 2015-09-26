using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Models.Api
{
    public class CleaningEventViewModel
    {
        public CleaningEventViewModel()
        {
            Assets = new List<AssetViewModel>();
        }

        public int Id { get; set; }
        public IEnumerable<AssetViewModel> Assets { get; set; }
    }
}
