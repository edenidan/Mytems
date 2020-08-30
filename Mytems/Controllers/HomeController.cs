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
            var productsArr =
            (from prod in db.Products
            orderby prod.NumberOfViews descending
            select prod).ToArray();

            List<Product> prodList = new List<Product>();
            for (int i = 0; i < productsArr.Count() && i < 6; i++)
            {
                prodList.Add(productsArr[i]);
            }

            return View(prodList.ToArray());
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

            User user = db.Users.Where(u => u.Username == username && u.Password == password).FirstOrDefault();
            if (user != null)
            {
                Session["User"] = user;
                return RedirectToAction("Dashboard");
            }
            ModelState.AddModelError("Password", "Incorrect username or password.");
            return View();
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            User user = Session["User"] as User;
            if (user is Customer)
                return RedirectToAction("Dashboard", "Customers");
            else if (user is Seller)
                return RedirectToAction("Dashboard", "Sellers");
            else if (user is Admin)
                return RedirectToAction("Dashboard", "Admins");
            
            return View("Index");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("User");
            return RedirectToAction("Index");
        }

        public ActionResult contact()
        {
            return View();
        }

    }
}
