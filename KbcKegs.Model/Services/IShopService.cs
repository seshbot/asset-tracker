using KbcKegs.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Model.Services
{
    public interface IShopService
    {
        Order FindOrderById(int orderId);
        Order FindOrder(string sourceId, string customerSourceId);
    }
}
