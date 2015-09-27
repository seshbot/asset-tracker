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
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private KbcDbContext db = new KbcDbContext();

        [Route("")]
        [HttpGet]
        public IEnumerable<OrderViewModel> GetOrderViewModels()
        {
            return db.Orders.Select(OrderViewModelExtensions.ToViewModel);
        }

        [Route("~/api/customers/{customerId}/orders")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<OrderViewModel>))]
        public IHttpActionResult GetCustomerOrderViewModels(int customerId)
        {
            var customer = db.Customers.Find(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer.Orders.Select(OrderViewModelExtensions.ToViewModel));
        }

        [Route("{id}", Name = "GetOrder")]
        [HttpGet]
        [ResponseType(typeof(OrderViewModel))]
        public IHttpActionResult GetOrder(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order.ToViewModel());
        }
        
        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, OrderViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vm.Id)
            {
                return BadRequest();
            }

            var order = db.Orders.Find(vm.Id);
            vm.UpdateDb(order);
            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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
        [ResponseType(typeof(OrderViewModel))]
        public IHttpActionResult PostOrder(OrderViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(vm.ToNewDb());
            db.SaveChanges();

            return CreatedAtRoute("GetOrder", new { id = vm.Id }, vm);
        }

        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(OrderViewModel))]
        public IHttpActionResult DeleteOrder(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order.ToViewModel());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}