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
        public ActionResult Index()
        {
            var rooms = db.Rooms.Include(r => r.Room_Catalog);
            return View(rooms.ToList());
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
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "roomNumber,typeId,roomStatus,operationStatus")] Room room)
        {
            if (ModelState.IsValid)
            {
                Room_Catalog rc = db.Room_Catalogs.SingleOrDefault(m => m.typeId == room.typeId);
                rc.quantityOfRooms++;

                db.Rooms.Add(room);
                db.Entry(rc).State = EntityState.Modified;
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
            return PartialView("../Rooms/_Edit", room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // POST: Rooms/Delete/5
        public ActionResult Delete(int id, string typeId)
        {
            try
            {
                Room room = db.Rooms.Find(id);
                Room_Catalog rc = db.Room_Catalogs.SingleOrDefault(m => m.typeId == typeId);
                rc.quantityOfRooms--;

                db.Rooms.Remove(room);
                db.Entry(rc).State = EntityState.Modified;
                db.SaveChanges();
                TempData["deleteSuccess"] = "<script>alert('Delete successfully');</script>";
            }
            catch (Exception)
            {
                TempData["deleteFail"] = "<script>alert('The type of room is used by other objects');</script>";
            }

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
