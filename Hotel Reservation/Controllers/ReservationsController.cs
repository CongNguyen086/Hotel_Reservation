using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel_Reservation.Models;
using System.Text;

namespace Hotel_Reservation.Controllers
{
    public class ReservationsController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Reservations
        public ActionResult Index()
        {
            var reservations = db.Reservations.Include(r => r.Registered_Customer);
            return View(reservations.ToList());
        }

        public ActionResult UpdateForm(int id)
        {
            Session["Update"] = id;
            return RedirectToAction("Index");
        }

        public ActionResult UpdateStatus(int id, string status)
        {
            Reservation rs = db.Reservations.FirstOrDefault(m => m.reservationId == id);
            if (rs != null)
            {
                Session["Update"] = null;
                rs.reservationStatus = status;
                db.Entry(rs).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Reservations/Details/5
        public ActionResult ReservationDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var rs = db.Reservation_Details.Where(m => m.reservationId == id);
            Reservation rs2 = db.Reservations.Find(id);

            decimal total = 0;

            foreach(var item in rs)
            {
                total += item.totalPrice;
            }

            ViewBag.CustomerName = rs2.customerName;
            ViewBag.Phone = rs2.phone;
            ViewBag.Total = total;
            return View(rs.ToList());
        }


        // GET: Reservations/Create
        //public ActionResult Create()
        //{
        //    ViewBag.customerId = new SelectList(db.Registered_Customers, "customerId", "customerName");
        //    return View();
        //}

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public RedirectToRouteResult Create()
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            Reservation rs = new Reservation();
            rs.customerId = Int32.Parse(Session["customerId"].ToString());
            rs.customerName = Session["customerName"].ToString();
            rs.phone = Session["phone"].ToString();
            rs.email = Session["email"].ToString();
            rs.reservationStatus = "Processing";
            rs.checkInDate = Convert.ToDateTime(Session["CheckIn"]);
            rs.checkOutDate = Convert.ToDateTime(Session["CheckOut"]);

            foreach (var item in giohang)
            {
                Reservation_Detail rd = new Reservation_Detail();
                rd.reservationId = rs.reservationId;
                rd.roomNumber = item.roomNumber;
                rd.numberOfTravelers = item.traveler;
                rd.extraFee = item.extraFee;
                rd.totalPrice = item.totalPrice;

                Room room = db.Rooms.Find(item.roomNumber);
                room.roomStatus = "Reserved";
                Room_Catalog rc = db.Room_Catalogs.SingleOrDefault(m => m.typeId == room.typeId);
                int room2 = db.Rooms.Where(m => m.typeId == rc.typeId).Where(m => m.roomStatus == "Available").Count();
                if(room2 == 0)
                {
                    rc.catalogStatus = "Inactive";
                    db.Entry(rc).State = EntityState.Modified;
                }

                db.Reservation_Details.Add(rd);
            }

            db.Reservations.Add(rs);
            db.SaveChanges();

            giohang.Clear();
            return RedirectToAction("Index", "Homes");
        }

        // GET: Reservations/Edit/5
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

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "reservationId,checkInDate,checkOutDate,customerId,customerName,phone,email,reservationStatus")] Reservation reservation)
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

        // GET: Reservations/Delete/5
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

        // POST: Reservations/Delete/5
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
