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

namespace KbcKegs.Controllers.Api
{
    [RoutePrefix("api/collections")]
    public class CollectionEventsController : ApiController
    {
        private KbcDbContext db = new KbcDbContext();
        private IAssetTypeRepository assetTypes;
        private IAssetRepository assets;
        private IInventoryService inventory;

        public CollectionEventsController()
        {
            assetTypes = new AssetTypeRepository(db);
            assets = new AssetRepository(db);
            var customers = new CustomerRepository(db);
            var orders = new OrderRepository(db);
            var events = new EventRepository(db);

            inventory = new InventoryService(assetTypes, assets, customers, orders, events);
        }

        [NonAction]
        private CollectionEventViewModel CreateViewModel(CollectionEvent evt)
        {
            return new CollectionEventViewModel
            {
                Id = evt.Id,
                DateTime = evt.DateTime,
                Assets = evt.Assets.Select(AssetViewModelExtensions.ToViewModel).ToList(),
            };
        }

        [NonAction]
        private CollectionEvent CreateNewEvent(CollectionEventViewModel vm)
        {
            return new CollectionEvent
            {
                DateTime = DateTime.UtcNow, // dont copy from vm - we are creating a new event
                Assets = vm.Assets.Select(a =>
                    inventory.MergeAsset(a.Id, a.SerialNumber, a.Description)).ToList(),
            };
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<CollectionEventViewModel> GetCollectionEvents()
        {
            return db.CollectionEvents.Select(CreateViewModel);
        }

        [Route("~/api/customers/{customerId}/collections/")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<CollectionEventViewModel>))]
        public IHttpActionResult GetCustomerCollectionEvents(int customerId)
        {
            var customer = db.Customers.Find(customerId);
            if (customer == null)
            {
                return NotFound();
            }
            
            return Ok(customer.Collections.Select(CreateViewModel));
        }

        [Route("{id}",Name = "GetCollectionEvent")]
        [HttpGet]
        [ResponseType(typeof(CollectionEventViewModel))]
        public IHttpActionResult GetCollectionEvent(int id)
        {
            var collectionEvent = db.CollectionEvents.Find(id);
            if (collectionEvent == null)
            {
                return NotFound();
            }

            var vm = CreateViewModel(collectionEvent);
            return Ok(vm);
        }
        
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(CollectionEventViewModel))]
        public IHttpActionResult PostCollectionEvent(CollectionEventViewModel collectionEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newEvent = CreateNewEvent(collectionEvent);

            inventory.HandleEvent(newEvent);

            var newEventViewModel = CreateViewModel(newEvent);

            return CreatedAtRoute("GetCollectionEvent", new { id = newEventViewModel.Id }, newEventViewModel);
        }
        
        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(CollectionEventViewModel))]
        public IHttpActionResult DeleteCollectionEvent(int id)
        {
            var collectionEvent = db.CollectionEvents.Find(id);
            if (collectionEvent == null)
            {
                return NotFound();
            }

            var vm = CreateViewModel(collectionEvent);

            db.CollectionEvents.Remove(collectionEvent);
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

        private bool CollectionEventExists(int id)
        {
            return db.CollectionEvents.Count(e => e.Id == id) > 0;
        }
    }
}