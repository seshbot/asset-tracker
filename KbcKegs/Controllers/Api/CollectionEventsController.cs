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
        private IAssetRepository assets;
        private IInventoryService inventory;

        public CollectionEventsController()
        {
            assets = new AssetRepository(db);
            var customers = new CustomerRepository(db);
            var orders = new OrderRepository(db);
            var events = new EventRepository(db);

            inventory = new InventoryService(assets, customers, orders, events);
        }

        [NonAction]
        private CollectionEventViewModel CreateViewModel(CollectionEvent evt)
        {
            return new CollectionEventViewModel
            {
                Id = evt.Id,
                CustomerId = evt.CustomerId,
                CustomerName = evt.Customer?.Name,
                Assets = evt.Assets.Select(AssetViewModelExtensions.ToViewModel).ToList(),
            };
        }

        [NonAction]
        private CollectionEvent CreateNewEvent(CollectionEventViewModel vm)
        {
            return new CollectionEvent
            {
                DateTime = DateTime.UtcNow,
                Assets = vm.Assets.Select(a => assets.GetById(a.Id)).ToList(),
                CustomerId = vm.CustomerId,
            };
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<CollectionEventViewModel> GetCollectionEvents()
        {
            return db.CollectionEvents.Select(CreateViewModel);
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

            collectionEvent.Id = newEvent.Id;

            return CreatedAtRoute("GetCollectionEvent", new { id = collectionEvent.Id }, collectionEvent);
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