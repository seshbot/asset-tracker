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
            DateTime = DateTime.Now;
        }

        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public IEnumerable<AssetViewModel> Assets { get; set; }
    }
}
