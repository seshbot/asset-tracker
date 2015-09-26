using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Models.Api
{
    public class DeliveryEventViewModel
    {
        public DeliveryEventViewModel()
        {
            Assets = new List<AssetViewModel>();
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderSourceId { get; set; }
        public string OrderCustomerName { get; set; }
        public IEnumerable<AssetViewModel> Assets { get; set; }
    }

    public class AssetViewModel
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
    }
}
