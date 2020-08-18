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
    public class SellersController : Controller
    {
        private MytemsDB db = new MytemsDB();

        // GET: Sellers
        public ActionResult Index()
        {
            // TODO check for permission (admin)
            return View(db.Sellers.ToList());
        }

        public ActionResult Dashboard()
        {
            User user = Session["User"] as User;
            if (user is Seller)
                return View();
            else return View("~/Views/Errors/Unauthorized.cshtml");
        }
        // GET: Sellers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Sellers.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // GET: Sellers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sellers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,FirstName,LastName,PhoneNumber,Location")] Seller seller)
        {
            ModelState.Remove("UserID");
            ModelState.Remove("JoinedAt");
            ModelState.Remove("Rating");
            ModelState.Remove("NumberOfRators");
            try
            {
                if (ModelState.IsValid)
                {
                    seller.UserID = 0;
                    seller.JoinedAt = DateTime.Now;
                    seller.Rating = null;
                    seller.NumberOfRators = 0;

                    db.Sellers.Add(seller);
                    db.SaveChanges();
                    Session["User"] = seller;
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

            return View(seller);
        }

        // GET: Sellers/Edit/5
        public ActionResult Edit(int? id)
        {
            // TODO check for permission (admin or this seller)
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Sellers.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            // TODO check for permission (admin or this seller)
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller sellerToUpdate = db.Sellers.Find(id);
            if (TryUpdateModel(sellerToUpdate, new[] { "Username", "Password", "FirstName", "LastName", "PhoneNumber", "Location" }))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException)
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "An error occurred while updating the database.");
                }
            }
            return View(sellerToUpdate);
        }

        // GET: Sellers/Delete/5
        public ActionResult Delete(int? id)
        {
            // TODO check for permission (admin or this seller)
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Sellers.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            return View(seller);
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            // TODO check for permission (admin or this seller)
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Sellers.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            db.Users.Remove(seller);
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
