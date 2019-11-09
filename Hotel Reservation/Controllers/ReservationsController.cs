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
    public class ReservationsController : Controller
    {
        private ModelContext db = new ModelContext();

        // Create Booking
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
                rd.numberOfTravelers = item.guest;
                rd.extraFee = item.extraFee;
                rd.totalPrice = item.itemTotalPrice;

                Room room = db.Rooms.Find(item.roomNumber);
                room.roomStatus = "Reserved";
                Room_Catalog rc = db.Room_Catalogs.SingleOrDefault(m => m.typeId == room.typeId);
                int room2 = db.Rooms.Where(m => m.typeId == rc.typeId).Where(m => m.roomStatus == "Available").Count();
                if (room2 == 0)
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
