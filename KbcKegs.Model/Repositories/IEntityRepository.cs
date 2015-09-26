using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Model.Repositories
{
    public interface IEntityRepository<T>
    {
        T GetById(int id);

        IQueryable<T> AsQueryable { get; }

        void Add(T entity, bool commit = true);

        void AddRange(IEnumerable<T> entities, bool commit = true);

        void Update(T entity, bool commit = true);

        void Remove(T entity, bool commit = true);
    }
}
