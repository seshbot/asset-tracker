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

namespace KbcKegs.Controllers.Api
{
    public class AssetTypesController : ApiController
    {
        private KbcDbContext db = new KbcDbContext();

        // GET: api/AssetTypes
        public IQueryable<AssetType> GetAssetTypes()
        {
            return db.AssetTypes;
        }

        // GET: api/AssetTypes/5
        [ResponseType(typeof(AssetType))]
        public IHttpActionResult GetAssetType(int id)
        {
            AssetType assetType = db.AssetTypes.Find(id);
            if (assetType == null)
            {
                return NotFound();
            }

            return Ok(assetType);
        }

        // PUT: api/AssetTypes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAssetType(int id, AssetType assetType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != assetType.Id)
            {
                return BadRequest();
            }

            db.Entry(assetType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetTypeExists(id))
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

        // POST: api/AssetTypes
        [ResponseType(typeof(AssetType))]
        public IHttpActionResult PostAssetType(AssetType assetType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AssetTypes.Add(assetType);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = assetType.Id }, assetType);
        }

        // DELETE: api/AssetTypes/5
        [ResponseType(typeof(AssetType))]
        public IHttpActionResult DeleteAssetType(int id)
        {
            AssetType assetType = db.AssetTypes.Find(id);
            if (assetType == null)
            {
                return NotFound();
            }

            db.AssetTypes.Remove(assetType);
            db.SaveChanges();

            return Ok(assetType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AssetTypeExists(int id)
        {
            return db.AssetTypes.Count(e => e.Id == id) > 0;
        }
    }
}