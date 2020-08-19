using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mytems.Models;
using Mytems.ViewModels.Products;

namespace Mytems.Controllers
{
    public class ProductsController : Controller
    {
        private MytemsDB db = new MytemsDB();

        // GET: Products
        public ActionResult Index(ProductSearchOptions searchOptions) // TODO: possibly search by seller
        {
            var searchedProducts = searchOptions
                .ApplyOn(db.Products)
                .Include(p => p.Seller)
                .Select(p => new IndexProduct().From(p))
                .ToList();
            ViewData["SearchOptions"] = searchOptions;
            return View(searchedProducts);
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
            return View(new DetailsProduct().From(product));
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            User user = Session["User"] as User;

            // check for permission
            if (!(user is Admin || user is Seller))
                return View("~/Views/Errors/Unauthorized.cshtml");

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateProduct createProduct, HttpPostedFileBase file)
        {
            User user = Session["User"] as User;

            // check for permission
            if (!(user is Admin || user is Seller))
                return View("~/Views/Errors/Unauthorized.cshtml");

            if (user is Seller) // remove validation for seller id field
                ModelState.Remove("SellerID");

            try
            {
                if (ModelState.IsValid)
                {
                    Product product = createProduct.ToProduct();
                    if (file != null)
                    {
                        string imagePath = "~/Static/" + Guid.NewGuid().ToString().Substring(0, 16) + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath(imagePath));
                        product.ImagePath = imagePath;
                    }

                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while adding the product, please contact an administrator if the problem persists.");
            }

            return View(createProduct);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            User user = Session["User"] as User;

            if (!(user is Admin) && !(user is Seller))
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = db.Products.Find(id);

            if (product == null)
                return HttpNotFound();
            if (!user.CanEditAndDelete(product))
                return View("~/Views/Errors/Unauthorized.cshtml");

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Name,Category,Price,Description,Sold")] Product product)
        {
            var productInDB = db.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
            if (product.Sold && !productInDB.Sold)
                product.SoldAt = DateTime.Now;

            // TODO do this with a viewmodel and check for permission (admin or the product's seller)
            if (ModelState.IsValid)
            {
                //db.Entry(product).State = EntityState.Modified;
                productInDB.Name = product.Name;
                productInDB.Price = product.Price;
                productInDB.Category = product.Category;
                productInDB.Description = product.Description;
                productInDB.Sold = product.Sold;

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
