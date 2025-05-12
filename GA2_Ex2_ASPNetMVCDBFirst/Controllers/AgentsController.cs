using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GA2_Ex2_ASPNetMVCDBFirst.Models;

namespace GA2_Ex2_ASPNetMVCDBFirst.Controllers
{
    public class AgentsController : Controller
    {
        private InventoryDBEntities db = new InventoryDBEntities();

        // GET: Agents (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Index()
        {
            return View(db.Agents.ToList());
        }

        // GET: Agents/Details/5 (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = db.Agents.Find(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        // GET: Agents/Create (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agents/Create (Admin-only)
        [AuthorizeAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AgentID,AgentName,Address")] Agent agent)
        {
            if (ModelState.IsValid)
            {
                db.Agents.Add(agent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agent);
        }

        // GET: Agents/Edit/5 (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = db.Agents.Find(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        // POST: Agents/Edit/5 (Admin-only)
        [AuthorizeAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AgentID,AgentName,Address")] Agent agent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agent);
        }

        // GET: Agents/Delete/5 (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = db.Agents.Find(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        // POST: Agents/Delete/5 (Admin-only)
        [AuthorizeAdmin]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agent agent = db.Agents.Find(id);
            db.Agents.Remove(agent);
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