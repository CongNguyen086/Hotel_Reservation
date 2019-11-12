using System;
using System.Collections;
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
            List<BookingItem> booking = Session["giohang"] as List<BookingItem>;
            Reservation rs = new Reservation();
            rs.customerId = Int32.Parse(Session["customerId"].ToString());
            rs.customerName = Session["customerName"].ToString();
            rs.phone = Session["phone"].ToString();
            rs.email = Session["email"].ToString();
            rs.reservationStatus = "Processing";
            rs.checkInDate = Convert.ToDateTime(Session["CheckIn"]);
            rs.checkOutDate = Convert.ToDateTime(Session["CheckOut"]);

            foreach (var item in booking)
            {
                Reservation_Detail rd = new Reservation_Detail();
                rd.reservationId = rs.reservationId;
                rd.roomNumber = item.roomNumber;
                rd.numberOfTravelers = item.guest;
                rd.extraFee = item.extraFee;
                rd.totalPrice = item.itemTotalPrice;

                Room room = db.Rooms.Find(item.roomNumber);
                room.roomStatus = "Reserved";
                //Room_Catalog rc = db.Room_Catalogs.SingleOrDefault(m => m.typeId == room.typeId);
                //int room2 = db.Rooms.Where(m => m.typeId == rc.typeId).Where(m => m.roomStatus == "Available").Count();
                //if (room2 == 0)
                //{
                //    rc.catalogStatus = "Inactive";
                //    db.Entry(rc).State = EntityState.Modified;
                //}

                db.Reservation_Details.Add(rd);
            }

            db.Reservations.Add(rs);
            db.SaveChanges();

            booking.Clear();
            return RedirectToAction("Index", "Homes");
        }

        public ActionResult SelectRoom(string checkIn, string checkOut)
        {
            var room_Catalogs = db.Room_Catalogs.Where(m => m.catalogStatus == "Active");
            var promotions = db.Promotions.Where(p => p.promotionStatus == "Processing");
            List<RoomCatalog_Promotion> activeRoomList = new List<RoomCatalog_Promotion>();
            RoomCatalog_Promotion activeRoom;
            if (checkIn != null)
            {
                Session["CheckIn"] = DateTime.ParseExact(checkIn, "dd/MM/yyyy", null);
                Session["CheckOut"] = DateTime.ParseExact(checkOut, "dd/MM/yyyy", null);
            }

            var isPromoted = false;
            foreach (var rc in room_Catalogs)
            {
                if (!isFullReserved(rc, checkIn, checkOut))
                {
                    var imageList = rc.Image_Details.Where(i => i.typeId.Equals(rc.typeId));
                    List<Image_Detail> imageList_Promotion = new List<Image_Detail>();
                    foreach (var i in imageList)
                    {
                        imageList_Promotion.Add(new Image_Detail
                        {
                            imageId = i.imageId,
                            image = i.image,
                            imageOrder = i.imageOrder,
                        });
                    }
                    foreach (var p in promotions)
                    {
                        if (p.appliedRoomType.Equals(rc.typeId))
                        {
                            isPromoted = true;
                            activeRoom = new RoomCatalog_Promotion()
                            {
                                typeId = rc.typeId,
                                typeName = rc.typeName,
                                price = rc.price,
                                description = rc.description,
                                quantityOfRooms = rc.quantityOfRooms,
                                numberOfAdults = rc.numberOfAdults,
                                numberOfChild = rc.numberOfChild,
                                extraFee = rc.extraFee,
                                catalogStatus = rc.catalogStatus,
                                promotionDescription = p.promotionDescription,
                                appliedRoomType = p.appliedRoomType,
                                roomDiscount = p.roomDiscount,
                                image = imageList_Promotion,
                            };
                            activeRoomList.Add(activeRoom);
                            break;
                        }
                    }
                    if (!isPromoted)
                    {
                        activeRoom = new RoomCatalog_Promotion()
                        {
                            typeId = rc.typeId,
                            typeName = rc.typeName,
                            price = rc.price,
                            description = rc.description,
                            quantityOfRooms = rc.quantityOfRooms,
                            numberOfAdults = rc.numberOfAdults,
                            numberOfChild = rc.numberOfChild,
                            extraFee = rc.extraFee,
                            catalogStatus = rc.catalogStatus,
                            promotionDescription = null,
                            appliedRoomType = null,
                            roomDiscount = null,
                            image = imageList_Promotion,
                        };
                        activeRoomList.Add(activeRoom);
                    }
                    else
                    {
                        isPromoted = false;
                    }
                }
            }

            return View(activeRoomList);
        }

        public Boolean isDateRangeOverlap(string checkIn, string checkOut, string start2, string end2)
        {
            if ((checkIn != null) && (checkOut != null))
            {
                var startDate1 = DateTime.ParseExact(checkIn, "dd/MM/yyyy", null);
                var endDate1 = DateTime.ParseExact(checkOut, "dd/MM/yyyy", null);
                var startDate2 = DateTime.ParseExact(start2, "dd/MM/yyyy", null);
                var endDate2 = DateTime.ParseExact(end2, "dd/MM/yyyy", null);
                var maxStart = startDate1 > startDate2 ? startDate1 : startDate2;
                var minEnd = endDate1 < endDate2 ? endDate1 : endDate2;
                return maxStart < minEnd;
            }
            return false;
        }

        public Boolean hasAvailableRoom(Room_Catalog rc)
        {
            var availableRoomList = db.Rooms.Where(r => r.typeId.Equals(rc.typeId)
                                                        && r.roomStatus.Equals("Available")
                                                        && r.operationStatus.Equals("Active")
                                                    ).FirstOrDefault();
            return availableRoomList != null ? true : false;
        }

        public Boolean isFullReserved(Room_Catalog rc, string checkIn, string checkOut)
        {
            if (!hasAvailableRoom(rc))
            {
                // get all reserve details that relate to rc
                var reservationDt = db.Reservation_Details.Where(rd => rd.Room.typeId.Equals(rc.typeId));
                var roomList = db.Rooms.Where(r => r.typeId.Equals(rc.typeId));
                ArrayList reservedRoomList = new ArrayList();
                var hasAvailableInDateRange = false;
                foreach (var rd in reservationDt)
                {
                    var reservation = db.Reservations.Find(rd.reservationId);
                    if (isDateRangeOverlap(checkIn, checkOut, reservation.checkInDate.ToString("dd/MM/yyyy"), reservation.checkOutDate.ToString("dd/MM/yyyy")))
                    {
                        if (!reservedRoomList.Contains(rd.roomNumber))
                        {
                            reservedRoomList.Add(rd.roomNumber);
                        }
                    }
                }
                if (reservedRoomList != null)
                {
                    foreach (var room in roomList)
                    {
                        if (!reservedRoomList.Contains(room.roomNumber) && room.operationStatus.Equals("Active"))
                        {
                            hasAvailableInDateRange = true;
                        }
                    }
                    if (!hasAvailableInDateRange)
                    {
                        return true;
                    }
                }
            }
            return false;
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
