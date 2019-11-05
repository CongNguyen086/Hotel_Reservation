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
    public class AdminReservation_DetailController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Reservation_Detail
        public ActionResult Index()
        {
            var reservation_Details = db.Reservation_Details.Include(r => r.Reservation).Include(r => r.Room);
            return View(reservation_Details.ToList());
        }

        // GET: Reservation_Detail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation_Detail reservation_Detail = db.Reservation_Details.Find(id);
            if (reservation_Detail == null)
            {
                return HttpNotFound();
            }
            return View(reservation_Detail);
        }

        // GET: Reservation_Detail/Create
        public ActionResult Create()
        {
            ViewBag.reservationId = new SelectList(db.Reservations, "reservationId", "customerName");
            ViewBag.roomNumber = new SelectList(db.Rooms, "roomNumber", "typeId");
            return View();
        }

        // POST: Reservation_Detail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "reservationId,roomNumber,numberOfTravelers,extraFee,totalPrice")] Reservation_Detail reservation_Detail)
        {
            if (ModelState.IsValid)
            {
                db.Reservation_Details.Add(reservation_Detail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.reservationId = new SelectList(db.Reservations, "reservationId", "customerName", reservation_Detail.reservationId);
            ViewBag.roomNumber = new SelectList(db.Rooms, "roomNumber", "typeId", reservation_Detail.roomNumber);
            return View(reservation_Detail);
        }

        // GET: Reservation_Detail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation_Detail reservation_Detail = db.Reservation_Details.Find(id);
            if (reservation_Detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.reservationId = new SelectList(db.Reservations, "reservationId", "customerName", reservation_Detail.reservationId);
            ViewBag.roomNumber = new SelectList(db.Rooms, "roomNumber", "typeId", reservation_Detail.roomNumber);
            return View(reservation_Detail);
        }

        // POST: Reservation_Detail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "reservationId,roomNumber,numberOfTravelers,extraFee,totalPrice")] Reservation_Detail reservation_Detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation_Detail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.reservationId = new SelectList(db.Reservations, "reservationId", "customerName", reservation_Detail.reservationId);
            ViewBag.roomNumber = new SelectList(db.Rooms, "roomNumber", "typeId", reservation_Detail.roomNumber);
            return View(reservation_Detail);
        }

        // GET: Reservation_Detail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation_Detail reservation_Detail = db.Reservation_Details.Find(id);
            if (reservation_Detail == null)
            {
                return HttpNotFound();
            }
            return View(reservation_Detail);
        }

        // POST: Reservation_Detail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation_Detail reservation_Detail = db.Reservation_Details.Find(id);
            db.Reservation_Details.Remove(reservation_Detail);
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
