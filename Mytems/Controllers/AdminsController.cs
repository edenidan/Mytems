﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Mytems.Models;
using Mytems.PresentationModels.Admins;

namespace Mytems.Controllers
{
    public class AdminsController : Controller
    {
        private MytemsDB db = new MytemsDB();

        // GET: Admins
        public ActionResult Index(AdminSearchOptions searchOptions)
        {
            if (!(Session["User"] is Admin))
                return View("~/Views/Errors/Unauthorized.cshtml");

            var searchedAdmins = searchOptions
                .ApplyOn(db.Admins)
                .ToList();

            ViewData["SearchOptions"] = searchOptions;
            return View(searchedAdmins);
        }

        public ActionResult Dashboard()
        {
            if (!(Session["User"] is Admin))
                return View("~/Views/Errors/Unauthorized.cshtml");
            ViewBag.columnGraphData = numberOfSalesPerDay(30);
            return View();
        }

        public string[] numberOfSalesPerDay(int days)
        {
            if (days > 30 || days < 0) days = 30;

            string[] retVal = new string[2];
            StringBuilder sb = new StringBuilder();

            sb.Append("[");
            for (int i = days - 1; i >= 0; i--)
            {
                sb.Append(DateTime.Today.AddDays(-i).Day + "." + DateTime.Today.AddDays(-i).Month + ",");
            }
            retVal[0] = sb.Remove(sb.Length - 1, 1).Append("]").ToString();

            sb.Clear();

            sb.Append("[");
            for (int i = days - 1; i >= 0; i--) {
                sb.Append((from prod in db.Products.ToList()
                           where prod.SoldAt.HasValue && (DateTime.Now - prod.SoldAt).Value.Days == i
                           select prod.ProductID).Count().ToString() + ",");
            }
            retVal[1] = sb.Remove(sb.Length - 1, 1).Append("]").ToString();

            return retVal;
        }

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (!(Session["User"] is Admin))
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Admin admin = db.Admins.Find(id);
            if (admin == null)
                return HttpNotFound();

            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            if (!(Session["User"] is Admin))
                return View("~/Views/Errors/Unauthorized.cshtml");

            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password")] Admin admin)
        {
            if (!(Session["User"] is Admin))
                return View("~/Views/Errors/Unauthorized.cshtml");

            ModelState.Remove("UserID");
            ModelState.Remove("JoinedAt");
            if (ModelState.IsValid)
            {
                admin.UserID = 0;
                admin.JoinedAt = DateTime.Now;

                if (db.Users.Any(u => u.Username == admin.Username))
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(admin);
                }

                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!(Session["User"] is Admin))
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Admin admin = db.Admins.Find(id);
            if (admin == null)
                return HttpNotFound();

            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (!(Session["User"] is Admin))
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Admin adminToUpdate = db.Admins.Find(id);
            if (adminToUpdate == null)
                return HttpNotFound();

            if (TryUpdateModel(adminToUpdate, new[] { "Username", "Password" }))
            {
                if (db.Users.Where(u => u.UserID != adminToUpdate.UserID).Any(u => u.Username == adminToUpdate.Username))
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(adminToUpdate);
                }
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "An error occurred while updating the database.");
                    return View(adminToUpdate);
                }
            }
            return View(adminToUpdate);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(Session["User"] is Admin))
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Admin admin = db.Admins.Find(id);
            if (admin == null)
                return HttpNotFound();

            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!(Session["User"] is Admin))
                return View("~/Views/Errors/Unauthorized.cshtml");
            Admin admin = db.Admins.Find(id);
            if (admin == null)
                return HttpNotFound();

            db.Admins.Remove(admin);
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
