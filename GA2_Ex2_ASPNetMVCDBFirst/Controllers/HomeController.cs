using System.Linq;
using System.Web.Mvc;
using GA2_Ex2_ASPNetMVCDBFirst.Models;

namespace GA2_Ex2_ASPNetMVCDBFirst.Controllers
{
    public class HomeController : Controller
    {
        private InventoryDBEntities db = new InventoryDBEntities();

        // GET: Home
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }

            var userName = Session["UserName"]?.ToString();
            var user = db.Users.FirstOrDefault(u => u.UserName == userName);
            if (user != null && user.UserName == "admin" && user.Email == "admin@gmail.com")
            {
                return View();
            }
            return RedirectToAction("Shopping", "Items");
        }

        // GET: Home/Login
        public ActionResult Login()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password && u.LockStatus == false);
                if (user != null)
                {
                    Session["UserID"] = user.UserID;
                    Session["UserName"] = user.UserName;
                    if (user.UserName == "admin" && user.Email == "admin@gmail.com")
                    {
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Shopping", "Items");
                }
                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(model);
        }

        // GET: Home/Register
        public ActionResult Register()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Home/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.FirstOrDefault(u => u.UserName == model.UserName || u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username or email already exists.");
                    return View(model);
                }

                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password,
                    LockStatus = false
                };
                db.Users.Add(user);
                db.SaveChanges();

                Session["UserID"] = user.UserID;
                Session["UserName"] = user.UserName;
                return RedirectToAction("Shopping", "Items");
            }
            return View(model);
        }

        // GET: Home/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
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