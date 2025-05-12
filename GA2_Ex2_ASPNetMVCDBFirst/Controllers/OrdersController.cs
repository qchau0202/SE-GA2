using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GA2_Ex2_ASPNetMVCDBFirst.Models;

namespace GA2_Ex2_ASPNetMVCDBFirst.Controllers
{
    public class OrdersController : Controller
    {
        private InventoryDBEntities db = new InventoryDBEntities();

        // GET: Orders (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Agent).Include(o => o.User);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5 (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Include(o => o.OrderDetails.Select(od => od.Item)).FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Create()
        {
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName");
            return View();
        }

        // POST: Orders/Create (Admin-only)
        [AuthorizeAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,OrderDate,AgentID,UserID,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.Status = "Pending";
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", order.AgentID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", order.UserID);
            return View(order);
        }

        // GET: Orders/Edit/5 (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", order.AgentID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", order.UserID);
            return View(order);
        }

        // POST: Orders/Edit/5 (Admin-only)
        [AuthorizeAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,OrderDate,AgentID,UserID,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", order.AgentID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", order.UserID);
            return View(order);
        }

        // GET: Orders/Delete/5 (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5 (Admin-only)
        [AuthorizeAdmin]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Include OrderDetails to fetch related records
            Order order = db.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            // Delete all associated OrderDetail records
            var orderDetails = order.OrderDetails.ToList();
            foreach (var detail in orderDetails)
            {
                db.OrderDetails.Remove(detail);
            }

            // Now delete the Order
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Orders/CreateOrder (Admin-only)
        [AuthorizeAdmin]
        public ActionResult CreateOrder()
        {
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName");
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName");
            var model = new OrderViewModel { OrderDate = DateTime.Now };
            model.OrderDetails.Add(new OrderDetailViewModel());
            return View(model);
        }

        // POST: Orders/CreateOrder (Admin-only)
        [AuthorizeAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = new Order
                {
                    OrderDate = model.OrderDate,
                    AgentID = db.Agents.First().AgentID,
                    UserID = (int)Session["UserID"],
                    Status = "Pending"
                };
                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var detail in model.OrderDetails)
                {
                    if (detail.ItemID > 0 && detail.Quantity > 0)
                    {
                        db.OrderDetails.Add(new OrderDetail
                        {
                            OrderID = order.OrderID,
                            ItemID = detail.ItemID,
                            Quantity = detail.Quantity,
                            UnitAmount = detail.UnitAmount
                        });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "AgentName", model.AgentID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", model.UserID);
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName");
            return View(model);
        }

        // GET: Orders/Report (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Report()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            var allOrders = db.Orders
                              .Include(o => o.OrderDetails.Select(od => od.Item))
                              .Include(o => o.Agent)
                              .Include(o => o.User)
                              .ToList();
            return View("Report", allOrders);
        }

        // GET: Orders/ReportDetails/5 (Admin-only)
        [AuthorizeAdmin]
        public ActionResult ReportDetails(int? id)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = db.Orders
                         .Include(o => o.OrderDetails.Select(od => od.Item))
                         .Include(o => o.Agent)
                         .Include(o => o.User)
                         .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound("Order not found.");
            }

            return View("ReportDetails", order);
        }

        // GET: Orders/PlaceOrder (User-facing)
        public ActionResult PlaceOrder()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName");
            var model = new OrderViewModel { OrderDate = DateTime.Now };
            var cart = Session["Cart"] as List<OrderDetailViewModel> ?? new List<OrderDetailViewModel>();
            model.OrderDetails = cart;
            if (!model.OrderDetails.Any())
            {
                model.OrderDetails.Add(new OrderDetailViewModel());
            }
            return View(model);
        }

        // POST: Orders/PlaceOrder (User-facing)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(OrderViewModel model)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var order = new Order
                {
                    OrderDate = model.OrderDate,
                    AgentID = db.Agents.First().AgentID, // Default agent
                    UserID = (int)Session["UserID"], // Set UserID
                    Status = "Pending"
                };
                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var detail in model.OrderDetails)
                {
                    if (detail.ItemID > 0 && detail.Quantity > 0)
                    {
                        db.OrderDetails.Add(new OrderDetail
                        {
                            OrderID = order.OrderID,
                            ItemID = detail.ItemID,
                            Quantity = detail.Quantity,
                            UnitAmount = detail.UnitAmount
                        });
                    }
                }
                db.SaveChanges();
                Session["Cart"] = null; // Clear cart
                return RedirectToAction("OrderHistory");
            }
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName");
            return View(model);
        }

        // GET: Orders/Cart (User-facing)
        public ActionResult Cart()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var cart = Session["Cart"] as List<OrderDetailViewModel> ?? new List<OrderDetailViewModel>();
            ViewBag.Items = db.Items.ToList();
            return View(cart);
        }

        // POST: Orders/AddToCart (User-facing)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int itemId, int quantity)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var item = db.Items.Find(itemId);
            if (item == null)
            {
                return HttpNotFound();
            }
            var cart = Session["Cart"] as List<OrderDetailViewModel> ?? new List<OrderDetailViewModel>();
            cart.Add(new OrderDetailViewModel
            {
                ItemID = itemId,
                ItemName = item.ItemName,
                Quantity = quantity,
                UnitAmount = item.UnitPrice ?? 0
            });
            Session["Cart"] = cart;
            TempData["SuccessMessage"] = $"{item.ItemName} has been added to your cart!";
            return RedirectToAction("Shopping", "Items");
        }

        // GET: Orders/OrderHistory (User-facing)
        public ActionResult OrderHistory()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var userId = (int)Session["UserID"];
            var orders = db.Orders
                .Where(o => o.UserID == userId)
                .Include(o => o.OrderDetails.Select(od => od.Item))
                .ToList();
            return View(orders);
        }

        // POST: Orders/CancelOrder/5 (User-facing)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelOrder(int id)
        {
            if (Session["UserID"] == null)
            {
                TempData["ErrorMessage"] = "Please log in to cancel an order.";
                return RedirectToAction("Login", "Home");
            }
            var userId = (int)Session["UserID"];
            var order = db.Orders.FirstOrDefault(o => o.OrderID == id && o.UserID == userId);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found or you do not have permission to cancel it.";
                return RedirectToAction("OrderHistory");
            }
            if (order.Status == "Cancelled" || order.Status == "Accepted")
            {
                TempData["ErrorMessage"] = $"Order #{id} cannot be cancelled because it is {order.Status.ToLower()}.";
                return RedirectToAction("OrderHistory");
            }
            order.Status = "Cancelled";
            db.SaveChanges();
            TempData["SuccessMessage"] = $"Order #{id} has been cancelled successfully.";
            return RedirectToAction("OrderHistory");
        }

        // GET: Orders/AcceptOrder/5 (Admin-only)
        [AuthorizeAdmin]
        public ActionResult AcceptOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Include(o => o.OrderDetails.Select(od => od.Item)).FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/AcceptOrder/5 (Admin-only)
        [AuthorizeAdmin]
        [HttpPost, ActionName("AcceptOrder")]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptOrderConfirmed(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null || order.Status != "Pending")
            {
                return HttpNotFound();
            }
            order.Status = "Accepted";
            db.SaveChanges();
            return RedirectToAction("Index");
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