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
        private IAssetTypeRepository _assetTypes;
        private IAssetRepository _assets;
        private ICustomerRepository _customers;
        private IOrderRepository _orders;
        private IEventRepository _events;

        public InventoryService(
            IAssetTypeRepository assetTypes,
            IAssetRepository assets,
            ICustomerRepository customers,
            IOrderRepository orders,
            IEventRepository events)
        {
            if (null == assetTypes) throw new ArgumentNullException("assetTypes");
            if (null == assets) throw new ArgumentNullException("assets");
            if (null == customers) throw new ArgumentNullException("customers");
            if (null == orders) throw new ArgumentNullException("orders");
            if (null == events) throw new ArgumentNullException("events");

            _assetTypes = assetTypes;
            _assets = assets;
            _customers = customers;
            _orders = orders;
            _events = events;
        }

        public Asset MergeAsset(int? id, string serialNumber, string description, AssetState state) // upsert
        {
            if (id.HasValue && id.Value > 0)
            {
                var asset = FindAssetById(id.Value);
                if (!string.IsNullOrEmpty(description))
                {
                    if (string.IsNullOrEmpty(asset.AssetType?.Description) || 
                        0 != string.Compare(description, asset.AssetType?.Description, true))
                    {
                        asset.AssetType = FindAssetTypeByDescription(description);
                    }
                }

                if (state != AssetState.Unspecified)
                {
                    asset.State = state;
                }

                _assets.Update(asset);
                
                return asset;
            }

            return CreateAsset(serialNumber, description, state);
        }

        public AssetType FindAssetTypeByDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return FindAssetTypeByDescription(AssetType.Unknown);
            }

            var result = _assetTypes.GetByDescription(description);
            if (null == result)
            {
                result = new AssetType { Description = description };
                _assetTypes.Add(result);
            }

            return result;
        }

        public Asset FindAssetById(int id)
        {
            return _assets.GetById(id);
        }

        public Asset CreateAsset(string serialNumber, string description, AssetState state)
        {
            var result = _assets.GetBySerialNumber(serialNumber);
            if (result == null)
            {
                var newAssetState = state == AssetState.Unspecified ? AssetState.Available : state;
                result = new Asset
                {
                    SerialNumber = serialNumber,
                    State = newAssetState,
                    AssetType = FindAssetTypeByDescription(description)
                };
                _assets.Add(result);
            }
            else
            {
                if (state != AssetState.Unspecified)
                {
                    result.State = state;
                    _assets.Update(result);
                }
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
