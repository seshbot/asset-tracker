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
    [RoutePrefix("api/cleaning")]
    public class CleaningEventsController : ApiController
    {
        private KbcDbContext db = new KbcDbContext();
        private IAssetRepository assets;
        private IInventoryService inventory;

        public CleaningEventsController()
        {
            assets = new AssetRepository(db);
            var customers = new CustomerRepository(db);
            var orders = new OrderRepository(db);
            var events = new EventRepository(db);

            inventory = new InventoryService(assets, customers, orders, events);
        }

        [NonAction]
        private CleaningEventViewModel CreateViewModel(CleaningEvent evt)
        {
            return new CleaningEventViewModel
            {
                Id = evt.Id,
                Assets = evt.Assets.Select(AssetViewModelExtensions.ToViewModel).ToList(),
            };
        }

        [NonAction]
        private CleaningEvent CreateNewEvent(CleaningEventViewModel vm)
        {
            return new CleaningEvent
            {
                DateTime = DateTime.UtcNow,
                Assets = vm.Assets.Select(a => assets.GetById(a.Id)).ToList(),
            };
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<CleaningEventViewModel> GetCleaningEvents()
        {
            return db.CleaningEvents.Select(CreateViewModel);
        }

        [Route("{id}",Name = "GetCleaningEvent")]
        [HttpGet]
        [ResponseType(typeof(CleaningEventViewModel))]
        public IHttpActionResult GetCleaningEvent(int id)
        {
            var cleaningEvent = db.CleaningEvents.Find(id);
            if (cleaningEvent == null)
            {
                return NotFound();
            }

            var vm = CreateViewModel(cleaningEvent);
            return Ok(vm);
        }
        
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(CleaningEventViewModel))]
        public IHttpActionResult PostCleaningEvent(CleaningEventViewModel cleaningEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newEvent = CreateNewEvent(cleaningEvent);

            inventory.HandleEvent(newEvent);

            cleaningEvent.Id = newEvent.Id;

            return CreatedAtRoute("GetCleaningEvent", new { id = cleaningEvent.Id }, cleaningEvent);
        }
        
        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(CleaningEventViewModel))]
        public IHttpActionResult DeleteCleaningEvent(int id)
        {
            var cleaningEvent = db.CleaningEvents.Find(id);
            if (cleaningEvent == null)
            {
                return NotFound();
            }

            var vm = CreateViewModel(cleaningEvent);

            db.CleaningEvents.Remove(cleaningEvent);
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

        private bool CleaningEventExists(int id)
        {
            return db.CleaningEvents.Count(e => e.Id == id) > 0;
        }
    }
}