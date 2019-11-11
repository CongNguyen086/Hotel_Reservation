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
    public class RoomsController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Rooms
        public ActionResult Index(string checkIn, string checkOut)
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
            if ((checkIn != null) && (checkOut != null)) {
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

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.typeId = new SelectList(db.Room_Catalogs, "typeId", "typeName");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "roomNumber,typeId,roomStatus,operationStatus")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.typeId = new SelectList(db.Room_Catalogs, "typeId", "typeName", room.typeId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.typeId = new SelectList(db.Room_Catalogs, "typeId", "typeName", room.typeId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "roomNumber,typeId,roomStatus,operationStatus")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.typeId = new SelectList(db.Room_Catalogs, "typeId", "typeName", room.typeId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
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
