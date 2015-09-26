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

        public WarehouseController()
        {
            events = new EventRepository(db);
        }

        // GET: Warehouse
        public ActionResult Index()
        {
            var since = DateTime.UtcNow.AddSeconds(-30);
            var vm = new WarehouseViewModel
            {
                RecentDeliveries = events.GetDeliveryEventsSince(since),
                RecentCollections = events.GetCollectionEventsSince(since),
            };

            return View(vm);
        }
    }
}