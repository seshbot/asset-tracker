using KbcKegs.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Model.Services
{
    public class InventoryService : IInventoryService
    {
        private IAssetRepository _assets;
        private ICustomerRepository _customers;
        private IOrderRepository _orders;
        private IEventRepository _events;

        public InventoryService(
            IAssetRepository assets,
            ICustomerRepository customers,
            IOrderRepository orders,
            IEventRepository events)
        {
            if (null == assets) throw new ArgumentNullException("assets");
            if (null == customers) throw new ArgumentNullException("customers");
            if (null == orders) throw new ArgumentNullException("orders");
            if (null == events) throw new ArgumentNullException("events");

            _assets = assets;
            _customers = customers;
            _orders = orders;
            _events = events;
        }

        public void HandleEvent(DeliveryEvent evt)
        {
            foreach (var asset in evt.Assets)
            {
                _events.Add(evt);

                asset.State = AssetState.WithCustomer;
                asset.History.Add(new AssetEventInfo
                {
                    AssetId = asset.Id,
                    EventId = evt.Id,
                    EventType = AssetEventType.Delivered,
                });

                _assets.Update(asset);
            }
        }

        public void HandleEvent(CollectionEvent evt)
        {
            foreach (var asset in evt.Assets)
            {
                _events.Add(evt);

                asset.State = AssetState.NeedsCleaning;
                asset.History.Add(new AssetEventInfo
                {
                    AssetId = asset.Id,
                    EventId = evt.Id,
                    EventType = AssetEventType.Collected,
                });

                _assets.Update(asset);
            }
        }

        public void HandleEvent(CleaningEvent evt)
        {
            foreach (var asset in evt.Assets)
            {
                _events.Add(evt);

                asset.State = AssetState.Available;
                asset.History.Add(new AssetEventInfo
                {
                    AssetId = asset.Id,
                    EventId = evt.Id,
                    EventType = AssetEventType.Collected,
                });

                _assets.Update(asset);
            }
        }
    }
}
