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
    public class RoomsController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Rooms
        public ActionResult Index(string txtRoom)
        {
            var room_Catalogs = db.Room_Catalogs.Where(m => m.catalogStatus == "Active");
            var promotions = db.Promotions.Where(p => p.promotionStatus == "Processing");
            List<RoomCatalog_Promotion> activeRoomList = new List<RoomCatalog_Promotion>();
            RoomCatalog_Promotion activeRoom;
            var isPromoted = false;
            foreach (var rc in room_Catalogs)
            {
                foreach(var p in promotions)
                {
                    if(p.appliedRoomType.Equals(rc.typeId))
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
                        };
                        activeRoomList.Add(activeRoom);
                        break;
                    }
                }
                if(!isPromoted)
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
                    };
                    activeRoomList.Add(activeRoom);
                } else
                {
                    isPromoted = false;
                }
            }
            if (!string.IsNullOrEmpty(txtRoom))
            {
                room_Catalogs = room_Catalogs.Where(b => b.typeName.Contains(txtRoom));
            }

            return View(activeRoomList);
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
