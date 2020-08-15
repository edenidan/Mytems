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
            ViewData["username"] = Session["username"];
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string data)
        {
            string username = Request.Form["username"];
            string password = Request.Form["password"];

            if(db.Users.Where(u=> u.Username == username && u.Password == password).Any())
            {
                Session["username"] = username;
                return RedirectToAction("Index");
            }

            return View();//TODO: pass an error message

        }

    }
}
