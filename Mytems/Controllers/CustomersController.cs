using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mytems.Models;

namespace Mytems.Controllers
{
    public class CustomersController : Controller
    {
        private MytemsDB db = new MytemsDB();

        // GET: Customers
        public ActionResult Index()
        {
            // TODO check for permission (admin)
            return View(db.Customers.ToList());
        }
        
        public ActionResult Dashboard()
        {
            //TODO pass real suggested for you products list (as SuggestedProducts in the viewbag)
            User user = Session["User"] as User;
            if (user is Customer)
            {
                ViewBag.SuggestedProducts = new Product[0];
                return View();
            }
            else return View("~/Views/Errors/Unauthorized.cshtml");
        }
        
        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            // TODO check for permission (admin or possibly this customer)
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,FirstName,LastName")] Customer customer)
        {
            ModelState.Remove("UserID");
            ModelState.Remove("JoinedAt");
            ModelState.Remove("CategoryViewsJson");
            try
            {
                if (ModelState.IsValid)
                {
                    customer.UserID = 0;
                    customer.JoinedAt = DateTime.Now;
                    customer.CategoryViewsJson = "{}";

                    if (db.Users.Where(a => a.Username == customer.Username).Any())
                    {
                        ModelState.AddModelError("Username", "This username is already taken.");
                        return View(customer);
                    }

                    db.Customers.Add(customer);
                    db.SaveChanges();
                    Session["User"] = customer;
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (DbEntityValidationException)
            {
                ModelState.AddModelError("Username", "This username is already taken.");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "An error occurred while updating the database.");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            // TODO check for permission (admin or this customer)
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            // TODO check for permission (admin or this customer)
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customerToUpdate = db.Customers.Find(id);
            if (TryUpdateModel(customerToUpdate, new[] { "Username", "Password", "FirstName", "LastName" }))
            {
                if (db.Users.Where(a => a.UserID != customerToUpdate.UserID && a.Username == customerToUpdate.Username).Any())
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(customerToUpdate);
                }
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "An error occurred while updating the database.");
                    return View(customerToUpdate);
                }
            }
            return View(customerToUpdate);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            // TODO check for permission (admin or this customer)
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            // TODO check for permission (admin or this customer)
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            db.Customers.Remove(customer);
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
