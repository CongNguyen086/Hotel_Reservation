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
    public class StaffReservationsController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: StaffReservations
        public ActionResult RoomManagementForBooking()
        {
            var reservations = db.Reservations.Include(r => r.Registered_Customer);
            return View();
        }

        // GET: StaffReservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: StaffReservations/Create
        public ActionResult Create()
        {
            ViewBag.customerId = new SelectList(db.Registered_Customers, "customerId", "customerName");
            return View();
        }

        // POST: StaffReservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "reservationId,checkInDate,checkOutDate,customerId,customerName,phone,email,numberOfAdult,numberOfChild,discount,extraFee,total,reservationStatus")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customerId = new SelectList(db.Registered_Customers, "customerId", "customerName", reservation.customerId);
            return View(reservation);
        }

        // GET: StaffReservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.customerId = new SelectList(db.Registered_Customers, "customerId", "customerName", reservation.customerId);
            return View(reservation);
        }

        // POST: StaffReservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "reservationId,checkInDate,checkOutDate,customerId,customerName,phone,email,numberOfAdult,numberOfChild,discount,extraFee,total,reservationStatus")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customerId = new SelectList(db.Registered_Customers, "customerId", "customerName", reservation.customerId);
            return View(reservation);
        }

        // GET: StaffReservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: StaffReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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
