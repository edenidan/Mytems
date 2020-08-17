﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mytems.Models;
using Mytems.ViewModels;

namespace Mytems.Controllers
{
    public class ProductsController : Controller
    {
        private MytemsDB db = new MytemsDB();

        // GET: Products
        public ActionResult Index(ProductSearchOptions searchOptions) // TODO: possibly search by seller
        {
            ViewData["SearchOptions"] = searchOptions;
            return View(searchOptions.ApplyOn(db.Products).ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            // TODO check for permission (admin or seller)
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Name,Category,Price,Description")] Product product, HttpPostedFileBase file)
        {
            ModelState.Remove("Sold");
            ModelState.Remove("NumberOfViews");
            ModelState.Remove("Seller");

            product.Sold = false;
            product.NumberOfViews = 0;

            string sellerUSername = Session["username"] as string;
            Seller seller = db.Sellers.FirstOrDefault(s => s.Username == sellerUSername);
            product.Seller = seller;

            product.Image = null;
            if (file != null)
            {
                string imageName = Guid.NewGuid().ToString().Substring(0,10)+file.FileName;
                product.Image = imageName;
                file.SaveAs(Server.MapPath($"~/Static/{imageName}"));
            }

            //TODO:
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            // TODO check for permission (admin or the product's seller)
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Name,Category,Price,Description,Sold,NumberOfViews")] Product product)
        {
            // TODO do this with a viewmodel and check for permission (admin or the product's seller)
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            // TODO check for permission (admin or the product's seller)
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            // TODO check for permission (admin or the product's seller)
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            db.Products.Remove(product);
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
