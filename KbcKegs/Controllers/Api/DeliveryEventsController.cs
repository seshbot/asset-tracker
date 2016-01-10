using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using KbcKegs.Data;
using KbcKegs.Model;
using KbcKegs.Model.Services;
using KbcKegs.Model.Repositories;
using KbcKegs.Models.Api;
using KbcKegs.Utils;

namespace KbcKegs.Controllers.Api
{
    public class TestViewModel
    {
        public string test { get; set; }
    }

    [RoutePrefix("api/deliveries")]
    public class DeliveryEventsController : ApiController
    {
        private KbcDbContext db = new KbcDbContext();
        private IAssetTypeRepository assetTypes;
        private IAssetRepository assets;
        private IOrderRepository orders;
        private ICustomerRepository customers;

        private IInventoryService inventory;
        private IShopService shop;

        public DeliveryEventsController()
        {
            assetTypes = new AssetTypeRepository(db);
            assets = new AssetRepository(db);
            orders = new OrderRepository(db);
            customers = new CustomerRepository(db);
            var events = new EventRepository(db);

            inventory = new InventoryService(assetTypes, assets, customers, orders, events);
            shop = new ShopService(orders, customers);
        }

        [NonAction]
        private DeliveryEventViewModel CreateViewModel(DeliveryEvent evt)
        {
            return new DeliveryEventViewModel
            {
                Id = evt.Id,
                DateTime = evt.DateTime,
                OrderFulfillments = evt.OrderFulfillments.Select(f => new OrderFulfillmentViewModel
                {
                    OrderId = f.OrderId,
                    OrderSourceId = f.Order.SourceId,
                    OrderCustomerSourceId = f.Order.Customer?.Name ?? "",
                    Assets = f.Assets.Select(AssetViewModelExtensions.ToViewModel).ToList(),
                }),
            };
        }

        [NonAction]
        private DeliveryEvent CreateNewEvent(DeliveryEventViewModel vm)
        {
            return new DeliveryEvent
            {
                DateTime = DateTime.UtcNow, // ignore vm DateTime, we are creating a new event
                OrderFulfillments = vm.OrderFulfillments.Select(f => new OrderFulfillment
                {
                    Assets = f.Assets.Select(a => 
                        inventory.MergeAsset(a.Id, a.SerialNumber, a.Description)).ToList(),
                    Order = shop.MergeOrder(f.OrderId, f.OrderSourceId, f.OrderCustomerSourceId),
                }).ToList(),
            };
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<DeliveryEventViewModel> GetDeliveryEvents()
        {
            return db.DeliveryEvents.Select(CreateViewModel);
        }

        [Route("~/api/orders/{orderId}/deliveries/")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<DeliveryEventViewModel>))]
        public IHttpActionResult GetOrderDeliveryEvents(int orderId)
        {
            var order = db.Orders.Find(orderId);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order.Deliveries.Select(CreateViewModel));
        }

        [Route("{id}",Name = "GetDeliveryEvent")]
        [HttpGet]
        [ResponseType(typeof(DeliveryEventViewModel))]
        public IHttpActionResult GetDeliveryEvent(int id)
        {
            var deliveryEvent = db.DeliveryEvents.Find(id);
            if (deliveryEvent == null)
            {
                return NotFound();
            }

            var vm = CreateViewModel(deliveryEvent);
            return Ok(vm);
        }
        
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(DeliveryEventViewModel))]
        public IHttpActionResult PostDeliveryEvent(DeliveryEventViewModel deliveryEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newEvent = CreateNewEvent(deliveryEvent);

            inventory.HandleEvent(newEvent);

            var newEventViewModel = CreateViewModel(newEvent);

            return CreatedAtRoute("GetDeliveryEvent", new { id = newEventViewModel.Id }, newEventViewModel);
        }
        
        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(DeliveryEventViewModel))]
        public IHttpActionResult DeleteDeliveryEvent(int id)
        {
            var deliveryEvent = db.DeliveryEvents.Find(id);
            if (deliveryEvent == null)
            {
                return NotFound();
            }

            var vm = CreateViewModel(deliveryEvent);

            db.DeliveryEvents.Remove(deliveryEvent);
            db.SaveChanges();

            return Ok(vm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeliveryEventExists(int id)
        {
            return db.DeliveryEvents.Count(e => e.Id == id) > 0;
        }
    }
}