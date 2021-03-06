﻿using KbcKegs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Model.Repositories
{
    public interface IOrderRepository : IEntityRepository<Order>
    {
        Order GetBySourceId(string sourceId);
    }
}
