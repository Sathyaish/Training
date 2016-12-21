using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeferredObjectMaterialization.Models;
using DeferredObjectMaterialization.ViewModels;
using b = DeferredObjectMaterialization.BusinessObjects;

namespace DeferredObjectMaterialization.Controllers
{
    public class ProductController : Controller
    {
        private DOMEntities db = new DOMEntities();

        //
        // GET: /Product/

        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        public ActionResult Buy(int id)
        {
            using (var context = new DOMEntities())
            {
                var productPaymentGateway = context.ProductPaymentGateways.Where(ppg => ppg.ProductId == id)
                    .SingleOrDefault();

                if (productPaymentGateway == null)
                {
                    return Content("Invalid product or product out of stock.");
                }

                return View(productPaymentGateway.Product);
            }
        }

        [HttpPost]
        public ActionResult PlaceOrder(OrderInfo orderInfo)
        {
            var dictionary = System.Web.HttpContext.Current.Application["ProductPaymentGatewayDictionary"] as Dictionary<int, Lazy<b::PaymentGateway>>;

            var gateway = dictionary[orderInfo.Id].Value;

            var result = gateway.Pay(orderInfo);

            if (!result.Successful) return Content(result.FailureReason);

            return Content("Thank you for your purhcase.");
        }

        //
        // GET: /Product/Details/5

        public ActionResult Details(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        //
        // GET: /Product/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}