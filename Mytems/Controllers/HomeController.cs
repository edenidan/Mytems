using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mytems.Models;

namespace Mytems.Controllers
{
    public class HomeController : Controller
    {
        private MytemsDB db = new MytemsDB();
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

            if (db.Users.Where(u => u.Username == username && u.Password == password).Any())
            {
                Session["username"] = username;
                return RedirectToAction("Dashboard");
            }
            ModelState.AddModelError("Password", "Incorrect username or password.");
            return View();
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            string username = Session["username"] as string;
            User user = db.Users.FirstOrDefault(u => u.Username == username);
            if (user is Customer)
                return RedirectToAction("Dashboard", "Customers");
            else if (user is Seller)
                return RedirectToAction("Dashboard", "Sellers");
            // TODO:
            //else if (user is Admin)
            //    return RedirectToAction("Dashboard", "Admin");
            else
                return View("Index");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["username"] = null;
            return RedirectToAction("Index");
        }

    }
}
