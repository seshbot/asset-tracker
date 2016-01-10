using System;
using System.Collections.Generic;
using System.Linq;
using KbcKegs.Model;
using System.Data.Entity;
using KbcKegs.Model.Repositories;

namespace KbcKegs.Data
{
    public class AssetTypeRepository : IAssetTypeRepository
    {
        private KbcDbContext _db;
        private DbSet<AssetType> _entities;

        public AssetTypeRepository(KbcDbContext db)
        {
            if (db == null)
                throw new ArgumentNullException("db");

            _db = db;
            _entities = _db.AssetTypes;
        }

        public IQueryable<AssetType> AsQueryable { get { return _entities; } }

        public void Add(AssetType entity, bool commit)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _entities.Add(entity);
            _db.SaveChanges();
        }

        public void AddRange(IEnumerable<AssetType> entities, bool commit)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            _entities.AddRange(entities);
            _db.SaveChanges();
        }

        public AssetType GetById(int id)
        {
            return AsQueryable.FirstOrDefault(e => e.Id == id);
        }

        public AssetType GetByDescription(string description)
        {
            return AsQueryable.FirstOrDefault(e => 0 == string.Compare(e.Description, description, true));
        }

        public void Remove(AssetType entity, bool commit)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _entities.Remove(entity);
            _db.SaveChanges();
        }

        public void Update(AssetType entity, bool commit)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _db.SaveChanges();
        }
    }
}
