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
using System.IO;

namespace Hotel_Reservation.Controllers
{
    public class Room_CatalogController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Room_Catalog
        public ActionResult Index()
        {
            var room_Catalogs = db.Room_Catalogs.Include(r => r.Promotion);
            return View(room_Catalogs.ToList());
        }

        // GET: Customer Room Catalog

        // GET: Room_Catalog/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_Catalog room_Catalog = db.Room_Catalogs.Find(id);
            if (room_Catalog == null)
            {
                return HttpNotFound();
            }
            return View(room_Catalog);
        }

        private string RandomString(int size, bool lowerCase)
        {

            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        // GET: Room_Catalog/Create
        public ActionResult Create()
        {
            ViewBag.promotionId = new SelectList(db.Promotions, "promotionId", "promotionDescription");
            return View();
        }

        // POST: Room_Catalog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "typeId,typeName,price,description,quantityOfRooms,numberOfAdults,extraFee,promotionId,catalogStatus")] Room_Catalog room_Catalog,
            HttpPostedFileBase file)
        {
            //ModelState.AddModelError("", room_Catalog.promotionId);
            if (ModelState.IsValid)
            {
                room_Catalog.promotionId = null;
                room_Catalog.quantityOfRooms = 0;
                db.Room_Catalogs.Add(room_Catalog);

                //db = new ModelContext();
                Image_Detail img = new Image_Detail();
                if (file.ContentLength > 0)
                {
                    string imgPath = Path.GetFileName(file.FileName);
                    string url = Path.Combine(Server.MapPath("~/img/"), imgPath);
                    file.SaveAs(url);
                    img.image = "/img/" + imgPath;
                }
                img.imageId = RandomString(5, true);
                img.typeId = room_Catalog.typeId;
                img.imageOrder = 1;
                db.Image_Details.Add(img);

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.promotionId = new SelectList(db.Promotions, "promotionId", "promotionDescription", room_Catalog.promotionId);
            return View(room_Catalog);
        }

        // GET: Room_Catalog/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_Catalog room_Catalog = db.Room_Catalogs.Find(id);

            if (room_Catalog == null)
            {
                return HttpNotFound();
            }
            ViewBag.promotionId = new SelectList(db.Promotions, "promotionId", "promotionDescription", room_Catalog.promotionId);
            return PartialView("../Room_Catalog/_Edit", room_Catalog);
        }

        // POST: Room_Catalog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "typeId,typeName,price,description,quantityOfRooms,numberOfAdults,extraFee,promotionId,catalogStatus")] Room_Catalog room_Catalog)
        {
            ModelState.AddModelError("", room_Catalog.typeId);
            if (ModelState.IsValid)
            {
                db.Entry(room_Catalog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.promotionId = new SelectList(db.Promotions, "promotionId", "promotionDescription", room_Catalog.promotionId);
            return View(room_Catalog);
        }

        // POST: Room_Catalog/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                Room_Catalog room_Catalog = db.Room_Catalogs.Find(id);
                db.Room_Catalogs.Remove(room_Catalog);
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
