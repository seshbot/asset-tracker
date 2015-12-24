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

        public Asset FindAssetById(int id)
        {
            return _assets.GetById(id);
        }

        public Asset CreateAsset(string serialNumber, AssetState state, string description)
        {
            var result = _assets.GetBySerialNumber(serialNumber);
            if (result == null)
            {
                result = new Asset { SerialNumber = serialNumber, State = state, Description = description };
                _assets.Add(result);
            }
            return result;
        }

        public void HandleEvent(DeliveryEvent evt)
        {
            foreach (var fulfillment in evt.OrderFulfillments)
            foreach (var asset in fulfillment.Assets)
            {
                asset.State = AssetState.WithCustomer;
                asset.WithCustomerId = fulfillment.Order.CustomerId;
                asset.History.Add(new AssetEventInfo
                {
                    AssetId = asset.Id,
                    EventId = evt.Id,
                    EventType = AssetEventType.Delivered,
                });

                _assets.Update(asset, commit: false);
            }

            _events.Add(evt);
        }

        public void HandleEvent(CollectionEvent evt)
        {
            foreach (var asset in evt.Assets)
            {
                asset.State = AssetState.NeedsCleaning;
                asset.WithCustomerId = null;
                asset.History.Add(new AssetEventInfo
                {
                    AssetId = asset.Id,
                    EventId = evt.Id,
                    EventType = AssetEventType.Collected,
                });

                _assets.Update(asset, commit: false);
            }

            _events.Add(evt);
        }

        public void HandleEvent(CleaningEvent evt)
        {
            foreach (var asset in evt.Assets)
            {
                asset.State = AssetState.Available;
                asset.History.Add(new AssetEventInfo
                {
                    AssetId = asset.Id,
                    EventId = evt.Id,
                    EventType = AssetEventType.Collected,
                });

                _assets.Update(asset, commit: false);
            }

            _events.Add(evt);
        }
    }
}
