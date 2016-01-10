using System;
using System.Collections.Generic;
using System.Linq;
using KbcKegs.Model;
using System.Data.Entity;
using KbcKegs.Model.Repositories;

namespace KbcKegs.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private KbcDbContext _db;
        private DbSet<Customer> _entities;

        public CustomerRepository(KbcDbContext db)
        {
            if (db == null)
                throw new ArgumentNullException("db");

            _db = db;
            _entities = _db.Customers;
        }

        public IQueryable<Customer> AsQueryable { get { return _db.Customers; } }

        public void Add(Customer entity, bool commit)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _entities.Add(entity);
            _db.SaveChanges();
        }

        public void AddRange(IEnumerable<Customer> entities, bool commit)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            _entities.AddRange(entities);
            _db.SaveChanges();
        }

        public Customer GetById(int id)
        {
            return _entities.Find(id);
        }

        public Customer GetBySourceId(string sourceId)
        {
            return _entities.FirstOrDefault(c => c.SourceId == sourceId);
        }

        public void Remove(Customer entity, bool commit)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _entities.Remove(entity);
            _db.SaveChanges();
        }

        public void Update(Customer entity, bool commit)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _db.SaveChanges();
        }
    }
}