﻿using System;
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
    public class HomesController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Homes
        public ActionResult Index(string txtRoom)
        {
            var room_Catalogs = db.Room_Catalogs.Where(m => m.catalogStatus == "Active");
            var promotions = db.Promotions.Where(p => (p.promotionStatus == "Processing") && (p.Room_Catalog.catalogStatus == "Active"));
            if (!string.IsNullOrEmpty(txtRoom))
            {
                room_Catalogs = room_Catalogs.Where(b => b.typeName.Contains(txtRoom));
            }

            return View(room_Catalogs.ToList());
        }

        // GET: Homes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_Catalog room_Catalog = db.Room_Catalogs.Find(id);
            var room = db.Rooms.Where(m => m.typeId == id);
            int count = 0;
            foreach (var item in room)
            { 
                if(item.roomStatus.Equals("Available") && item.operationStatus.Equals("Active"))
                {
                    count++;
                }
            }

            if(count == 0)
            {
                ViewBag.Status = "Reserved";
            } else
            {
                ViewBag.Status = "Available";
            }
            ViewBag.Availability = count;
            if (room_Catalog == null)
            {
                return HttpNotFound();
            }
            return View(room_Catalog);
        }

        // GET: Homes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Homes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "typeId,typeName,price,description,quantityOfRooms,numberOfAdults,extraFee,isPromoted,catalogStatus")] Room_Catalog room_Catalog)
        {
            if (ModelState.IsValid)
            {
                db.Room_Catalogs.Add(room_Catalog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room_Catalog);
        }

        // GET: Homes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_Catalog room_Catalog = db.Room_Catalogs.Find(id);
            if (room_Catalog == null)
            {
                return HttpNotFound();
            }
            return View(room_Catalog);
        }

        // POST: Homes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "typeId,typeName,price,description,quantityOfRooms,numberOfAdults,extraFee,isPromoted,catalogStatus")] Room_Catalog room_Catalog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room_Catalog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room_Catalog);
        }

        // GET: Homes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_Catalog room_Catalog = db.Room_Catalogs.Find(id);
            if (room_Catalog == null)
            {
                return HttpNotFound();
            }
            return View(room_Catalog);
        }

        // POST: Homes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Room_Catalog room_Catalog = db.Room_Catalogs.Find(id);
            db.Room_Catalogs.Remove(room_Catalog);
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
