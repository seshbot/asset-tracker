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

        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        void Update(T entity);

        void Remove(T entity);
    }
}
