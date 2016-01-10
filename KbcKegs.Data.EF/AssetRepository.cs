using System;
using System.Collections.Generic;
using System.Linq;
using KbcKegs.Model;
using System.Data.Entity;
using KbcKegs.Model.Repositories;

namespace KbcKegs.Data
{
    public class AssetRepository : IAssetRepository
    {
        private KbcDbContext _db;
        private DbSet<Asset> _entities;

        public AssetRepository(KbcDbContext db)
        {
            if (db == null)
                throw new ArgumentNullException("db");

            _db = db;
            _entities = _db.Assets;
        }
        
        public IQueryable<Asset> AsQueryable
        {
            get
            {
                return _entities.Include(e => e.AssetType);
            }
        }

        public void Add(Asset entity, bool commit)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _entities.Add(entity);
            _db.SaveChanges();
        }

        public void AddRange(IEnumerable<Asset> entities, bool commit)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            _entities.AddRange(entities);
            _db.SaveChanges();
        }

        public Asset GetById(int id)
        {
            return AsQueryable.FirstOrDefault(e => e.Id == id);
        }

        public Asset GetBySerialNumber(string serialNumber)
        {
            return AsQueryable.FirstOrDefault(e => 0 == string.Compare(e.SerialNumber, serialNumber, true));
        }

        public void Remove(Asset entity, bool commit)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _entities.Remove(entity);
            _db.SaveChanges();
        }

        public void Update(Asset entity, bool commit)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _db.SaveChanges();
        }
    }
}