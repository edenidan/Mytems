using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mytems.Controllers
{
    public class HomeController : Controller
    {
        private Models.MytemsDB db = new Models.MytemsDB();
        // GET: Home
        public ActionResult Index()
        {
            //ViewData["username"] = Session["username"];
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        [ValidateAntiForgeryToken]
        public ActionResult LoginPost()
        {
            string username = Request.Form["Username"];
            string password = Request.Form["Password"];

            if(db.Users.Where(u=> u.Username == username && u.Password == password).Any())
            {
                Session["username"] = username;
                return RedirectToAction("Dashboard");
            }
            ModelState.AddModelError("Username", "Incorrect username or password.");
            return View();
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            string username = Session["username"] as string;
            if (db.Customers.Where(c => c.Username == username).Any())
                return RedirectToAction("Dashboard", "Customers");

            if (db.Sellers.Where(s => s.Username == username).Any())
                return RedirectToAction("Dashboard", "Sellers");

            //TODO:
            //if (db.admins.Where(c => c.Username == Session["username"]).Any())
                //return RedirectToAction("Dashboard", "Customer");
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["username"] = null;
            return RedirectToAction("Index");
        }

    }
}
