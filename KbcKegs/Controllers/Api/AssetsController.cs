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

namespace KbcKegs.Controllers.Api
{
    [RoutePrefix("api/assets")]
    public class AssetsController : ApiController
    {
        private KbcDbContext db = new KbcDbContext();

        [Route("")]
        [HttpGet]
        public IEnumerable<AssetViewModel> GetAssetViewModels()
        {
            return db.Assets.Select(AssetViewModelExtensions.ToViewModel);
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

        // PUT: api/Assets/5
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

            var asset = db.Assets.Find(vm.Id);
            vm.UpdateDb(asset);
            db.Entry(asset).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
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

        // POST: api/Assets
        [ResponseType(typeof(AssetViewModel))]
        public IHttpActionResult PostAsset(AssetViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Assets.Add(vm.ToNewDb());
            db.SaveChanges();

            return CreatedAtRoute("GetAsset", new { id = vm.Id }, vm);
        }

        // DELETE: api/Assets/5
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