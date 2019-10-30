using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel_Reservation.Models;

namespace Hotel_Reservation.Controllers
{
    public class PromotionsController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Promotions
        public ActionResult Index()
        {
            var promotions = db.Promotions.Include(p => p.Room_Catalog);
            return View(promotions.ToList());
        }

        // GET: Promotions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promotion promotion = db.Promotions.Find(id);
            if (promotion == null)
            {
                return HttpNotFound();
            }
            return View(promotion);
        }

        // GET: Promotions/Create
        public ActionResult Create()
        {
            ViewBag.appliedRoomType = new SelectList(db.Room_Catalogs, "typeId", "typeName");
            return View();
        }

        // POST: Promotions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "promotionId,promotionDescription,appliedRoomType,roomDiscount,startDate,endDate,promotionStatus")] Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                var rc = db.Room_Catalogs.SingleOrDefault(i => i.typeId.Equals(promotion.appliedRoomType));
                if(rc != null)
                {
                    if(rc.promotionId == null)
                    {
                        rc.promotionId = promotion.promotionId;

                        db.Promotions.Add(promotion);
                        db.Entry(rc).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    } else
                    {
                        ModelState.AddModelError("", "The current promotion has not finished yet. Please check the current promotion's status");
                    }
                }
            }

            ViewBag.appliedRoomType = new SelectList(db.Room_Catalogs, "typeId", "typeName", promotion.appliedRoomType);
            return View(promotion);
        }

        // GET: Promotions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Promotion promotion = db.Promotions.Find(id);
            if (promotion == null)
            {
                return HttpNotFound();
            }
            ViewBag.appliedRoomType = new SelectList(db.Room_Catalogs, "typeId", "typeName", promotion.appliedRoomType);
            return PartialView("../Promotions/_Edit", promotion);
        }

        // POST: Promotions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "promotionId,promotionDescription,appliedRoomType,roomDiscount,startDate,endDate,promotionStatus")] Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                var rc = db.Room_Catalogs.SingleOrDefault(i => i.promotionId.Equals(promotion.promotionId));
                if(rc != null && promotion.promotionStatus.Equals("Overdued"))
                {
                    rc.promotionId = null;
                    db.Entry(rc).State = EntityState.Modified;
                }

                db.Entry(promotion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.appliedRoomType = new SelectList(db.Room_Catalogs, "typeId", "typeName", promotion.appliedRoomType);
            return View(promotion);
        }

        // POST: Promotions/Delete/5
        public ActionResult Delete(string id)
        {
            Promotion promotion = db.Promotions.Find(id);
            db.Promotions.Remove(promotion);
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
