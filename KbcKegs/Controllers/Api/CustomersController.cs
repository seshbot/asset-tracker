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
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
        private KbcDbContext db = new KbcDbContext();

        [Route("")]
        [HttpGet]
        public IEnumerable<CustomerViewModel> GetCustomerViewModels()
        {
            return db.Customers.Select(CustomerViewModelExtensions.ToViewModel);
        }

        [Route("{id}", Name = "GetCustomer")]
        [HttpGet]
        [ResponseType(typeof(CustomerViewModel))]
        public IHttpActionResult GetCustomer(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer.ToViewModel());
        }
        
        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, CustomerViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vm.Id)
            {
                return BadRequest();
            }

            var customer = db.Customers.Find(vm.Id);
            vm.UpdateDb(customer);
            db.Entry(customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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
        [ResponseType(typeof(CustomerViewModel))]
        public IHttpActionResult PostCustomer(CustomerViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(vm.ToNewDb());
            db.SaveChanges();

            return CreatedAtRoute("GetCustomer", new { id = vm.Id }, vm);
        }

        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(CustomerViewModel))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer.ToViewModel());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.Id == id) > 0;
        }
    }
}