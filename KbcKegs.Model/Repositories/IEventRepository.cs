using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Model.Repositories
{
    public interface IEventRepository
    {
        void Add(DeliveryEvent evt);
        void Add(CollectionEvent evt);
        void Add(CleaningEvent evt);

        IQueryable<DeliveryEvent> GetDeliveryEventsSince(DateTime since);
        IQueryable<CollectionEvent> GetCollectionEventsSince(DateTime since);
        IQueryable<CleaningEvent> GetCleaningEventsSince(DateTime since);
    }
}
