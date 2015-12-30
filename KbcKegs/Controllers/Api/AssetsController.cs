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
using KbcKegs.Models.Api;
using KbcKegs.Model.Services;

namespace KbcKegs.Controllers.Api
{
    [RoutePrefix("api/assets")]
    public class AssetsController : ApiController
    {
        private KbcDbContext db = new KbcDbContext();
        private IInventoryService _inventory;

        public AssetsController()
        {
            var assetTypes = new AssetTypeRepository(db);
            var assets = new AssetRepository(db);
            var orders = new OrderRepository(db);
            var customers = new CustomerRepository(db);
            var events = new EventRepository(db);

            _inventory = new InventoryService(assetTypes, assets, customers, orders, events);
        }

        [Route("")]
        [HttpGet]
        public IEnumerable<AssetViewModel> GetAssetViewModels()
        {
            return db.Assets.Select(AssetViewModelExtensions.ToViewModel);
        }

        [Route("~/api/customers/{customerId}/assets")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<AssetViewModel>))]
        public IHttpActionResult GetCustomerAssetViewModels(int customerId)
        {
            var customer = db.Customers.Find(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer.Assets.Select(AssetViewModelExtensions.ToViewModel));
        }

        [Route("{id}", Name = "GetAsset")]
        [HttpGet]
        [ResponseType(typeof(AssetViewModel))]
        public IHttpActionResult GetAsset(int id)
        {
            var asset = db.Assets.Find(id);
            if (asset == null)
            {
                return NotFound();
            }

            return Ok(asset.ToViewModel());
        }
        
        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAsset(int id, AssetViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vm.Id)
            {
                return BadRequest();
            }

            try
            {
                _inventory.MergeAsset(vm.Id, vm.SerialNumber, vm.Description);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(AssetViewModel))]
        public IHttpActionResult PostAsset(AssetViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var asset = _inventory.CreateAsset(vm.SerialNumber, vm.Description);
            vm.Id = asset.Id;

            return CreatedAtRoute("GetAsset", new { id = vm.Id }, vm);
        }

        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(AssetViewModel))]
        public IHttpActionResult DeleteAsset(int id)
        {
            var asset = db.Assets.Find(id);
            if (asset == null)
            {
                return NotFound();
            }

            db.Assets.Remove(asset);
            db.SaveChanges();

            return Ok(asset.ToViewModel());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AssetExists(int id)
        {
            return db.Assets.Count(e => e.Id == id) > 0;
        }
    }
}