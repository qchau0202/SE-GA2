using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GA2_Ex2_ASPNetMVCDBFirst.Models;

namespace GA2_Ex2_ASPNetMVCDBFirst.Controllers
{
    public class UsersController : Controller
    {
        private InventoryDBEntities db = new InventoryDBEntities();

        // GET: Users (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5 (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create (Admin-only)
        [AuthorizeAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserName,Email,Password,LockStatus")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Edit/5 (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5 (Admin-only)
        [AuthorizeAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserName,Email,Password,LockStatus")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5 (Admin-only)
        [AuthorizeAdmin]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (user.UserName == "admin" && user.Email == "admin@gmail.com")
            {
                TempData["Error"] = "Cannot delete the admin account.";
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // POST: Users/Delete/5 (Admin-only)
        [AuthorizeAdmin]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            if (user.UserName == "admin" && user.Email == "admin@gmail.com")
            {
                TempData["Error"] = "Cannot delete the admin account.";
                return RedirectToAction("Index");
            }
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Users/Profile (User-only)
        public ActionResult Profile()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int userId = (int)Session["UserID"];
            User user = db.Users.Find(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsEditing = Request.QueryString["edit"] == "true" || (Request.Form != null && Request.Form["IsEditing"] == "true");
            return View(user);
        }

        // POST: Users/Profile (User-only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile([Bind(Include = "UserID,UserName,Email")] User user)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                var existingUser = db.Users.FirstOrDefault(u => (u.UserName == user.UserName || u.Email == user.Email) && u.UserID != user.UserID);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username or email already exists.");
                    return View(user);
                }

                var dbUser = db.Users.Find(user.UserID);
                if (dbUser == null)
                {
                    return HttpNotFound();
                }

                dbUser.UserName = user.UserName;
                dbUser.Email = user.Email;
                db.Entry(dbUser).State = EntityState.Modified;
                db.SaveChanges();

                Session["UserName"] = user.UserName; // Update session
                TempData["Success"] = "Profile updated successfully.";
                return RedirectToAction("Profile");
            }
            return View(user);
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