using System.Linq;
using System.Web.Mvc;
using GA2_Ex2_ASPNetMVCDBFirst.Models;

namespace GA2_Ex2_ASPNetMVCDBFirst.Controllers
{
    public class ReportsController : Controller
    {
        private InventoryDBEntities db = new InventoryDBEntities();

        // GET: Reports/Filter (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Filter()
        {
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName");
            return View(new FilterViewModel());
        }

        // POST: Reports/Filter (Admin-only)
        [AuthorizeAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filter(FilterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.FilterType == "BestItems")
                {
                    model.BestItems = db.OrderDetails
                        .GroupBy(od => od.Item)
                        .Select(g => new BestItem
                        {
                            ItemID = g.Key.ItemID,
                            ItemName = g.Key.ItemName,
                            TotalQuantity = g.Sum(od => od.Quantity),
                            TotalRevenue = (decimal)g.Sum(od => od.Quantity * od.UnitAmount)
                        })
                        .OrderByDescending(b => b.TotalQuantity)
                        .Take(10)
                        .ToList();
                }
                else if (model.FilterType == "ItemsByAgent" && model.AgentID.HasValue)
                {
                    model.ItemsByAgent = db.OrderDetails
                        .Where(od => od.Order.AgentID == model.AgentID)
                        .GroupBy(od => od.Item)
                        .Select(g => new AgentItem
                        {
                            ItemID = g.Key.ItemID,
                            ItemName = g.Key.ItemName,
                            Quantity = g.Sum(od => od.Quantity)
                        })
                        .OrderByDescending(i => i.Quantity)
                        .ToList();
                }
                else if (model.FilterType == "AgentPurchases" && model.AgentID.HasValue)
                {
                    model.AgentPurchases = db.Orders
                        .Where(o => o.AgentID == model.AgentID)
                        .Select(o => new AgentOrder
                        {
                            OrderID = o.OrderID,
                            OrderDate = o.OrderDate,
                            Items = o.OrderDetails.Select(od => new AgentItem
                            {
                                ItemID = od.ItemID ?? 0,
                                ItemName = od.Item.ItemName,
                                Quantity = od.Quantity
                            }).ToList()
                        })
                        .OrderBy(o => o.OrderDate)
                        .ToList();
                }
            }
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", model.AgentID);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}