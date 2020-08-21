using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
        public ActionResult Index(ProductSearchOptions searchOptions)
        {
            var searchedProducts = searchOptions
                .ApplyOn(db.Products)
                .Include(p => p.Seller)
                .ToList()
                .Select(p => new IndexProduct(p));
            ViewData["SearchOptions"] = searchOptions;
            return View(searchedProducts);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = db.Products.Where(p => p.ProductID == id).Include(p => p.Seller).FirstOrDefault();
            if (product == null)
                return HttpNotFound();

            return View(new DetailsProduct(product));
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
                    if (user is Seller)
                        product.SellerID = user.UserID;

                    if (file != null)
                    {
                        string imagePath = "/Static/" + Guid.NewGuid().ToString().Substring(0, 16) + Path.GetExtension(file.FileName);
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

            return View(createProduct.ToProduct());
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            User user = Session["User"] as User;

            // check for permission
            if (!(user is Admin) && !(user is Seller))
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = db.Products.Find(id);

            if (product == null)
                return HttpNotFound();
            // check for permission
            if (!user.CanEditAndDeleteProduct(product))
                return View("~/Views/Errors/Unauthorized.cshtml");

            ViewBag.ImagePath = product.ImagePath;
            ViewBag.SellerID = new SelectList(db.Sellers, "UserID", "Username");
            return View(new EditProduct(product));
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditProduct editProduct, HttpPostedFileBase file)
        {
            User user = Session["User"] as User;

            var productInDB = db.Products.Find(editProduct.ProductID);
            if (productInDB == null)
                return HttpNotFound();

            // check for permission
            if (user == null || !user.CanEditAndDeleteProduct(productInDB))
                return View("~/Views/Errors/Unauthorized.cshtml");

            if (user is Seller) // remove validation for seller id field
                ModelState.Remove("SellerID");

            try
            {
                if (ModelState.IsValid)
                {
                    productInDB.Name = editProduct.Name;
                    productInDB.Price = editProduct.Price;
                    productInDB.Category = editProduct.Category;
                    productInDB.Description = editProduct.Description;
                    if (editProduct.Sold && !productInDB.Sold)
                        productInDB.SoldAt = DateTime.Now;
                    else if (!editProduct.Sold && productInDB.Sold)
                        productInDB.SoldAt = null;
                    productInDB.Sold = editProduct.Sold;

                    // TODO: Delete old image
                    if (file != null)
                    {
                        string imagePath = "/Static/" + Guid.NewGuid().ToString().Substring(0, 16) + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath(imagePath));
                        productInDB.ImagePath = imagePath;
                    }

                    if (user is Admin)
                        productInDB.SellerID = editProduct.SellerID.Value;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "An error occurred while adding the product, please contact an administrator if the problem persists.");
            }

            ViewBag.ImagePath = productInDB.ImagePath;
            ViewBag.SellerID = new SelectList(db.Sellers, "UserID", "Username");
            return View(editProduct);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = db.Products.Where(p => p.ProductID == id).Include(p => p.Seller).FirstOrDefault();
            if (product == null)
                return HttpNotFound();
            
            User user = Session["User"] as User;
            // check for permission
            if (user == null || !user.CanEditAndDeleteProduct(product))
                return View("~/Views/Errors/Unauthorized.cshtml");

            return View(new DetailsProduct(product));
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();

            User user = Session["User"] as User;
            // check for permission
            if (user == null || !user.CanEditAndDeleteProduct(product))
                return View("~/Views/Errors/Unauthorized.cshtml");

            // TODO: Delete image
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
