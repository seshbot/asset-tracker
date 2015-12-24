using KbcKegs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Model.Repositories
{
    public interface IAssetRepository : IEntityRepository<Asset>
    {
        Asset GetBySerialNumber(string serialNumber);
    }
}
