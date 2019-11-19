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
        private string strCart = "giohang";
        // Create Booking
        public RedirectToRouteResult Create()
        {
            List<BookingItem> booking = Session[strCart] as List<BookingItem>;
            Reservation rs = new Reservation();
            rs.customerId = Int32.Parse(Session["customerId"].ToString());
            rs.customerName = Session["customerName"].ToString();
            rs.phone = Session["phone"].ToString();
            rs.email = Session["email"].ToString();
            rs.checkInDate = Convert.ToDateTime(Session["bookingCheckIn"]);
            rs.checkOutDate = Convert.ToDateTime(Session["bookingCheckOut"]);
            rs.numberOfAdult = (Int32)Session["adults"];
            rs.numberOfChild = (Int32)Session["children"];
            rs.reservationStatus = "Reserved";

            foreach (var item in booking)
            {
                Reservation_Detail rd = new Reservation_Detail();
                rd.reservationId = rs.reservationId;
                rd.typeId = item.typeId;
                rd.roomNumber = 0;
                rd.discount = item.discount;
                rd.totalPrice = item.itemTotalPrice;

                Room room = db.Rooms.Find(item.typeId);
                room.roomStatus = "Processing";
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
            // Get the left list in View
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
                var availableRoomInDateRange = GetAvailableRoomInDateRange(rc, checkIn, checkOut);
                if (availableRoomInDateRange != 0)
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
                                numberOfAvailableRoom = availableRoomInDateRange,
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
                            numberOfAvailableRoom = availableRoomInDateRange,
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
            ViewData["ActiveRoomList"] = activeRoomList;
            // Create model for the right list in View
            //BookingViewModel bookingView = new BookingViewModel();
            //var bookingList = (List<BookingItem>)Session["DynamicBookingList"];
            return View();
        }

        public Boolean IsDateRangeOverlap(string checkIn, string checkOut, string start2, string end2)
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

        public int GetAvailableRoom(Room_Catalog rc)
        {
            var availableRoom = db.Rooms.Where(r => r.typeId.Equals(rc.typeId)
                                                        && r.roomStatus.Equals("Available")
                                                        && r.operationStatus.Equals("Active")
                                                    ).FirstOrDefault();
            return availableRoom != null ? availableRoom.roomNumber : 0;
        }

        public int GetAvailableRoomInDateRange(Room_Catalog rc, string checkIn, string checkOut)
        {
            var availableRoom = GetAvailableRoom(rc);
            if (availableRoom == 0)
            {
                // get all reserve details that relate to rc
                var reservationDt = db.Reservation_Details.Where(rd => rd.Room.typeId.Equals(rc.typeId));
                var roomList = db.Rooms.Where(r => r.typeId.Equals(rc.typeId));
                ArrayList reservedRoomList = new ArrayList();
                // Add RoomNumber to reservedList that has date-range overlap
                foreach (var rd in reservationDt)
                {
                    var reservation = db.Reservations.Find(rd.reservationId);
                    if (IsDateRangeOverlap(checkIn, checkOut, reservation.checkInDate.ToString("dd/MM/yyyy"), reservation.checkOutDate.ToString("dd/MM/yyyy")))
                    {
                        // Check duplicated RoomNumber in reservedList
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
                            availableRoom = room.roomNumber;
                        }
                    }
                }
            }
            return availableRoom;
        }

        // Return temp booking list
        public JsonResult CreateBookingItem(string id, string name)
        {
            if (Session["tempBooking"] == null)
            {
                Session["tempBooking"] = new List<TempBooking>();
            }

            List<TempBooking> tempBookings = (List<TempBooking>)Session["tempBooking"];
            tempBookings.Add(new TempBooking
            {
                typeId = id,
                typeName = name,
                numberOfRoom = 1, 
            });
            return Json(tempBookings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // View reservation
        public ActionResult MakeBooking(BookingViewModel bookingView)
        {
            Session["tempBooking"] = null;
            Session[strCart] = new List<BookingItem>();
            List<BookingItem> booking = Session[strCart] as List<BookingItem>;

            Session["bookingCheckIn"] = bookingView.checkIn;
            Session["bookingCheckOut"] = bookingView.checkOut;
            Session["adults"] = bookingView.numberOfAdult;
            Session["children"] = bookingView.numberOfChild;
            var nights = (bookingView.checkOut - bookingView.checkIn).Days;
            var bookingList = bookingView.bookingItems;

            foreach (var item in bookingList) {
                Room_Catalog rc = db.Room_Catalogs.Find(item.typeId);
                Image_Detail img = db.Image_Details.FirstOrDefault(m => m.typeId == rc.typeId);
                decimal discount = 0;
                var itemPromotion = "";
                var promotion = db.Promotions.FirstOrDefault(p => (p.appliedRoomType.Equals(rc.typeId))
                                                                    && (p.promotionStatus.Equals("Processing")));
                if (promotion != null)
                {
                    discount = promotion.roomDiscount ?? 0;
                    itemPromotion = promotion.promotionDescription;
                }
                BookingItem newItem = new BookingItem()
                {
                    typeId = item.typeId,
                    image = img.image,
                    typeName = rc.typeName,
                    unitPrice = rc.price,
                    night = 2,
                    extraFee = 0,
                    discount = Math.Round(rc.price * discount, 1),
                    promotion = itemPromotion,
                };

                booking.Add(newItem);
            }
            return RedirectToAction("ViewBooking");
        }

        public ActionResult ViewBooking()
        {
            List<BookingItem> booking = Session[strCart] as List<BookingItem>;
            Console.Write("{0}", Session[strCart]);
            if (booking != null)
            {
                var extraFee = booking.Select(m => m.extraFee).Sum();
                var discount = booking.Select(m => m.discount).Sum();
                ViewBag.Subtotal = booking.Select(m => m.itemTotalPrice).Sum();
                ViewBag.Total = ViewBag.Subtotal + extraFee - discount;
            }
            return View(booking);
        }

        public RedirectToRouteResult UpdateDate(string checkInFake, string checkOutFake)
        {
            Session["CheckIn"] = DateTime.ParseExact(checkInFake, "dd/MM/yyyy", null);
            Session["CheckOut"] = DateTime.ParseExact(checkOutFake, "dd/MM/yyyy", null);
            return RedirectToAction("SelectRoom", new { checkIn = checkInFake, checkOut = checkOutFake });
        }

        public RedirectToRouteResult DeleteItem(string typeId)
        {
            List<BookingItem> giohang = Session[strCart] as List<BookingItem>;
            BookingItem item = giohang.FirstOrDefault(m => m.typeId.Equals(typeId));
            if (item != null)
            {
                giohang.Remove(item);
            }
            return RedirectToAction("SelectRoom");
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
