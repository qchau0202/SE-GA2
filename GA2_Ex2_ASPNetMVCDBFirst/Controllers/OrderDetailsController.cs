using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GA2_Ex2_ASPNetMVCDBFirst.Models;

namespace GA2_Ex2_ASPNetMVCDBFirst.Controllers
{
    public class OrderDetailsController : Controller
    {
        private InventoryDBEntities db = new InventoryDBEntities();

        // GET: OrderDetails (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Index()
        {
            var orderDetails = db.OrderDetails.Include(o => o.Item).Include(o => o.Order);
            return View(orderDetails.ToList());
        }

        // GET: OrderDetails/Details/5 (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // GET: OrderDetails/Create (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Create()
        {
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName");
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID");
            return View();
        }

        // POST: OrderDetails/Create (Admin-only)
        [AuthorizeAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OrderID,ItemID,Quantity,UnitAmount")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName", orderDetail.ItemID);
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", orderDetail.OrderID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5 (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName", orderDetail.ItemID);
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", orderDetail.OrderID);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5 (Admin-only)
        [AuthorizeAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderID,ItemID,Quantity,UnitAmount")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName", orderDetail.ItemID);
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", orderDetail.OrderID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5 (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5 (Admin-only)
        [AuthorizeAdmin]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderDetail);
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