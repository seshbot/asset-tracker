using KbcKegs.Model;
using KbcKegs.Model.Repositories;
using System.Data.Entity;
using System;
using System.Linq;

namespace KbcKegs.Data
{
    public class EventRepository : IEventRepository
    {
        private KbcDbContext _db;
        private DbSet<DeliveryEvent> _deliveryEvents;
        private DbSet<CollectionEvent> _collectionEvents;
        private DbSet<CleaningEvent> _cleaningEvents;

        public EventRepository(KbcDbContext db)
        {
            _db = db;
            _deliveryEvents = _db.DeliveryEvents;
            _collectionEvents = _db.CollectionEvents;
            _cleaningEvents = _db.CleaningEvents;
        }

        public void Add(DeliveryEvent evt)
        {
            if (null == evt)
                throw new ArgumentNullException("evt");

            _deliveryEvents.Add(evt);
            _db.SaveChanges();
        }

        public void Add(CollectionEvent evt)
        {
            if (null == evt)
                throw new ArgumentNullException("evt");

            _collectionEvents.Add(evt);
            _db.SaveChanges();
        }

        public void Add(CleaningEvent evt)
        {
            if (null == evt)
                throw new ArgumentNullException("evt");

            _cleaningEvents.Add(evt);
            _db.SaveChanges();
        }

        public IQueryable<DeliveryEvent> GetDeliveryEventsSince(DateTime since)
        {
            return _deliveryEvents.Where(e => DbFunctions.DiffSeconds(since, e.DateTime) > 0);
        }

        public IQueryable<CollectionEvent> GetCollectionEventsSince(DateTime since)
        {
            return _collectionEvents.Where(e => DbFunctions.DiffSeconds(since, e.DateTime) > 0);
        }

        public IQueryable<CleaningEvent> GetCleaningEventsSince(DateTime since)
        {
            return _cleaningEvents.Where(e => DbFunctions.DiffSeconds(since, e.DateTime) > 0);
        }
    }
}