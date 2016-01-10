using System;
using System.Collections.Generic;
using System.Linq;
using KbcKegs.Model;
using System.Data.Entity;
using KbcKegs.Model.Repositories;

namespace KbcKegs.Data
{
    public class OrderRepository : IOrderRepository
    {
        private KbcDbContext _db;
        private DbSet<Order> _entities;

        public OrderRepository(KbcDbContext db)
        {
            if (db == null)
                throw new ArgumentNullException("db");

            _db = db;
            _entities = _db.Orders;
        }

        public IQueryable<Order> AsQueryable { get { return _db.Orders; } }

        public void Add(Order entity, bool commit)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _entities.Add(entity);
            _db.SaveChanges();
        }

        public void AddRange(IEnumerable<Order> entities, bool commit)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            _entities.AddRange(entities);
            _db.SaveChanges();
        }

        public Order GetById(int id)
        {
            return _entities.Find(id);
        }

        public Order GetBySourceId(string sourceId)
        {
            return AsQueryable.FirstOrDefault(o => o.SourceId.Equals(sourceId, StringComparison.InvariantCultureIgnoreCase));
        }

        public void Remove(Order entity, bool commit)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _entities.Remove(entity);
            _db.SaveChanges();
        }

        public void Update(Order entity, bool commit)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _db.SaveChanges();
        }
    }
}