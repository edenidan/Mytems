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

            return View();//TODO: pass an error message
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
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
