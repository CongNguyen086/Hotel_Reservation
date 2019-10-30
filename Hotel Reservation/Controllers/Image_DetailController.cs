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
    public class Image_DetailController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Image_Detail
        public ActionResult Index()
        {
            var image_Details = db.Image_Details.Include(i => i.Room_Catalog);
            return View(image_Details.ToList());
        }

        // GET: Image_Detail/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image_Detail image_Detail = db.Image_Details.Find(id);
            if (image_Detail == null)
            {
                return HttpNotFound();
            }
            return View(image_Detail);
        }

        // GET: Image_Detail/Create
        public ActionResult Create()
        {
            ViewBag.typeId = new SelectList(db.Room_Catalogs, "typeId", "typeName");
            return View();
        }

        // POST: Image_Detail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "imageId,typeId,image,imageOrder")] Image_Detail image_Detail)
        {
            if (ModelState.IsValid)
            {
                db.Image_Details.Add(image_Detail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.typeId = new SelectList(db.Room_Catalogs, "typeId", "typeName", image_Detail.typeId);
            return View(image_Detail);
        }

        // GET: Image_Detail/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image_Detail image_Detail = db.Image_Details.Find(id);
            if (image_Detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.typeId = new SelectList(db.Room_Catalogs, "typeId", "typeName", image_Detail.typeId);
            return View(image_Detail);
        }

        // POST: Image_Detail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "imageId,typeId,image,imageOrder")] Image_Detail image_Detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(image_Detail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.typeId = new SelectList(db.Room_Catalogs, "typeId", "typeName", image_Detail.typeId);
            return View(image_Detail);
        }

        // GET: Image_Detail/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image_Detail image_Detail = db.Image_Details.Find(id);
            if (image_Detail == null)
            {
                return HttpNotFound();
            }
            return View(image_Detail);
        }

        // POST: Image_Detail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Image_Detail image_Detail = db.Image_Details.Find(id);
            db.Image_Details.Remove(image_Detail);
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
