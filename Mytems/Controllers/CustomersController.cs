﻿using System;
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
using Mytems.PresentationModels.Customers;

namespace Mytems.Controllers
{
    public class CustomersController : Controller
    {
        private MytemsDB db = new MytemsDB();

        // GET: Customers
        public ActionResult Index(CustomerSearchOptions searchOptions)
        {
            if (!(Session["User"] is Admin))
                return View("~/Views/Errors/Unauthorized.cshtml");

            var searchedCustomers = searchOptions
                .ApplyOn(db.Customers)
                .ToList();

            ViewData["SearchOptions"] = searchOptions;
            return View(searchedCustomers);
        }

        public ActionResult Dashboard()
        {
            Customer customer = Session["User"] as Customer;
            if (customer == null)
                return View("~/Views/Errors/Unauthorized.cshtml");

            ViewBag.SuggestedProducts = GetSuggestions(customer, 8);
            return View();
        }

        //AI
        private List<Product> GetSuggestions(Customer customer, int count)
        {
            List<Product> result = new List<Product>();
            var categoryViews = customer.CategoryViews;

            // All the categories ordered by views
            var orderedCategories = ((Category[])Enum.GetValues(typeof(Category)))
                .GroupBy(c => categoryViews.ContainsKey(c) ? categoryViews[c] : 0)
                .OrderByDescending(group => group.Key)
                .SelectMany(group => group);

            foreach (Category category in orderedCategories)
            {
                if (result.Count >= count)
                    break;

                var mostViewedProductsInCategory = db.Products
                    .Where(p => p.Category == category)
                    .OrderByDescending(p => p.NumberOfViews)
                    .Take(count - result.Count)
                    .ToList();
                result.AddRange(mostViewedProductsInCategory);
            }

            //if (result.Count < count) // fill with fake products if there aren't enough
            //    result.AddRange(Enumerable.Repeat(new PresentationModels.Products.CreateProduct
            //    {
            //        Name = "Fake product",
            //        Description = "Fake Description"
            //    }.ToProduct(), count - result.Count));

            return result;
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            User user = Session["User"] as User;
            if (!(user is Admin) && user?.UserID != id)
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Customer customer = db.Users.Find(id) as Customer;
            if (customer == null)
                return HttpNotFound();

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

                    if (db.Users.Any(a => a.Username == customer.Username))
                    {
                        ModelState.AddModelError("Username", "This username is already taken.");
                        return View(customer);
                    }

                    db.Customers.Add(customer);
                    db.SaveChanges();

                    if (Session["User"] is Admin)
                        return RedirectToAction("Index", "Customers");

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
            User user = Session["User"] as User;
            if (!(user is Admin) && user?.UserID != id)
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Customer customer = db.Users.Find(id) as Customer;
            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            User user = Session["User"] as User;
            if (!(user is Admin) && user?.UserID != id)
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Customer customerToUpdate = db.Customers.Find(id);
            if (TryUpdateModel(customerToUpdate, new[] { "Username", "Password", "FirstName", "LastName" }))
            {
                if (db.Users.Where(u => u.UserID != customerToUpdate.UserID).Any(u => u.Username == customerToUpdate.Username))
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
            User user = Session["User"] as User;
            if (!(user is Admin) && user?.UserID != id)
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Customer customer = db.Customers.Find(id);
            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            User user = Session["User"] as User;
            if (!(user is Admin) && user?.UserID != id)
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Customer customer = db.Customers.Find(id);
            if (customer == null)
                return HttpNotFound();

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
