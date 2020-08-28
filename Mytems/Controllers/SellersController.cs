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
using Mytems.PresentationModels.Sellers;
using Newtonsoft.Json.Linq;

namespace Mytems.Controllers
{
    public class SellersController : Controller
    {
        private MytemsDB db = new MytemsDB();

        // GET: Sellers
        public ActionResult Index(SellerSearchOptions searchOptions)
        {
            if (!(Session["User"] is Admin))
                return View("~/Views/Errors/Unauthorized.cshtml");

            var searchedSellers = searchOptions
                .ApplyOn(db.Sellers)
                .ToList();

            ViewData["SearchOptions"] = searchOptions;
            return View(searchedSellers);
        }

        public ActionResult Dashboard()
        {
            if (!(Session["User"] is Seller))
                return View("~/Views/Errors/Unauthorized.cshtml");

            ViewBag.salesPerDayData = NumberOfSalesPerDay(7);
            ViewBag.categoryMoneyValueData = CategoryMoneyValue();
            ViewBag.bestSellersByTotalSalesMoney = BestSellersByTotalSalesMoney(3);

            return View();
        }

        public string NumberOfSalesPerDay(int days)
        {
            if (!(Session["User"] is Seller))
                return "Access Denied";

            if (days < 0) days = 7;

            JObject jobj = new JObject();
            JArray jarr = new JArray();


            for (int i = days - 1; i >= 0; i--)
            {
                jarr.Add(DateTime.Today.AddDays(-i).Day + "." + DateTime.Today.AddDays(-i).Month);
            }
            jobj.Add("labels", jarr);

            jarr = new JArray();

            for (int i = days - 1; i >= 0; i--)
            {
                jarr.Add((from prod in db.Products.ToList()
                          where prod.SellerID == (Session["User"] as User).UserID && prod.SoldAt.HasValue && (DateTime.Now - prod.SoldAt).Value.Days == i
                          select prod.ProductID).Count().ToString());
            }
            jobj.Add("data", jarr);

            return jobj.ToString();
        }

        public string CategoryMoneyValue()
        {
            if (!(Session["User"] is Seller))
                return "Access Denied";

            JObject jsonObj = new JObject();
            JArray jsonArrCategories = new JArray();
            JArray jsonArrSums = new JArray();
            Dictionary<Category, decimal> sumsDict = new Dictionary<Category, decimal>();

            // at the end of this loop, jsonArrCategories has all the names of the categories
            // and sumsDict has all the categories linked to the number 0 (initializing the sums)
            for (int i = 0; i < Enum.GetNames(typeof(Category)).Length; i++)
            {
                jsonArrCategories.Add(Enum.GetNames(typeof(Category))[i]);
                sumsDict[(Category)Enum.GetValues(typeof(Category)).GetValue(i)] = 0;
            }

            foreach (var element in
                from prod in db.Products.ToList()
                where prod.SellerID == (Session["User"] as User).UserID
                group prod by prod.Category into g
                orderby g.Key
                select new { category = g.Key, sum = g.Sum(p => p.Price) })
            {
                sumsDict[element.category] = element.sum;
            }

            for (int i = 0; i < Enum.GetNames(typeof(Category)).Length; i++)
            {
                jsonArrSums.Add(sumsDict[(Category)i]);
            }

            jsonObj.Add("labels", jsonArrCategories);
            jsonObj.Add("data", jsonArrSums);

            return jsonObj.ToString();
        }

        public string BestSellersByTotalSalesMoney(int numOfSellers)
        {
            if (!(Session["User"] is Seller))
                return "Access Denied";

            if (numOfSellers < 0) numOfSellers = 3;

            JObject jobj = new JObject();
            JArray jsonArrSellerNames = new JArray();
            JArray jsonArrSellerTotalSalesMoney = new JArray();

            int i = 0;
            foreach (var element in
                from prod in db.Products
                where prod.Sold == true
                join seller in db.Sellers on prod.SellerID equals seller.UserID
                group new { prod, seller } by seller.UserID into g
                orderby g.Key
                select new { sellerID = g.Select(e => e.seller.UserID).FirstOrDefault(), sellerName = g.Select(e => e.seller.Username).Distinct(), sum = g.Sum(e => e.prod.Price) })
            {
                if (i < numOfSellers && element.sellerID != (Session["User"] as User).UserID)
                {
                    jsonArrSellerNames.Add(element.sellerName);
                    jsonArrSellerTotalSalesMoney.Add(element.sum);
                    i++;
                }
                if(element.sellerID == (Session["User"] as User).UserID)
                {
                    jsonArrSellerNames.AddFirst(element.sellerName.FirstOrDefault() + " (you)");
                    jsonArrSellerTotalSalesMoney.AddFirst(element.sum);
                }
            }

            jobj.Add("labels", jsonArrSellerNames);
            jobj.Add("data", jsonArrSellerTotalSalesMoney);

            return jobj.ToString();
        }

        // GET: Sellers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Seller seller = db.Users.Find(id) as Seller;
            if (seller == null)
                return HttpNotFound();

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

                    if (db.Users.Any(u => u.Username == seller.Username))
                    {
                        ModelState.AddModelError("Username", "This username is already taken.");
                        return View(seller);
                    }

                    db.Sellers.Add(seller);
                    db.SaveChanges();
                    Session["User"] = seller;
                    return RedirectToAction("Dashboard", "Home");
                }
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
            User user = Session["User"] as User;
            if (!(user is Admin) && user?.UserID != id)
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Seller seller = db.Users.Find(id) as Seller;
            if (seller == null)
                return HttpNotFound();

            return View(seller);
        }

        // POST: Sellers/Edit/5
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

            Seller sellerToUpdate = db.Sellers.Find(id);
            if (TryUpdateModel(sellerToUpdate, new[] { "Username", "Password", "FirstName", "LastName", "PhoneNumber", "Location" }))
            {
                if (db.Users.Where(u => u.UserID != sellerToUpdate.UserID).Any(u => u.Username == sellerToUpdate.Username))
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(sellerToUpdate);
                }
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "An error occurred while updating the database.");
                    return View(sellerToUpdate);
                }
            }
            return View(sellerToUpdate);
        }

        // GET: Sellers/Delete/5
        public ActionResult Delete(int? id)
        {
            User user = Session["User"] as User;
            if (!(user is Admin) && user?.UserID != id)
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Seller seller = db.Sellers.Find(id);
            if (seller == null)
                return HttpNotFound();

            return View(seller);
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            User user = Session["User"] as User;
            if (!(user is Admin) && user?.UserID != id)
                return View("~/Views/Errors/Unauthorized.cshtml");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Seller seller = db.Sellers.Find(id);
            if (seller == null)
                return HttpNotFound();

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
