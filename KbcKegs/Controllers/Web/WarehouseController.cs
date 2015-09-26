using KbcKegs.Data;
using KbcKegs.Model.Repositories;
using KbcKegs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KbcKegs.Controllers.Web
{
    [Authorize]
    public class WarehouseController : Controller
    {
        private KbcDbContext db = new KbcDbContext();
        private IEventRepository events;
        private IAssetRepository assets;
        private ICustomerRepository customers;
        private IOrderRepository orders;

        public WarehouseController()
        {
            events = new EventRepository(db);
            assets = new AssetRepository(db);
            customers = new CustomerRepository(db);
            orders = new OrderRepository(db);
        }

        // GET: Warehouse
        public ActionResult Index()
        {
            var since = DateTime.UtcNow.AddDays(-1);
            var vm = new WarehouseViewModel
            {
                RecentDeliveries = events.GetDeliveryEventsSince(since),
                RecentCollections = events.GetCollectionEventsSince(since),
                RecentCleanings = events.GetCleaningEventsSince(since),
                Customers = customers.AsQueryable.ToList(),
                Orders = orders.AsQueryable.ToList(),
                Assets = assets.AsQueryable.Where(a => a.State != Model.AssetState.Retired).ToList(),
            };

            return View(vm);
        }
    }
}