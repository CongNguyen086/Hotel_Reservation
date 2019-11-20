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
    public class AdminChartsController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: AdminCharts
        public ActionResult Dashboard()
        {
            return View(db.ChartItems.ToList());
        }

        // GET: AdminCharts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChartItem chartItem = db.ChartItems.Find(id);
            if (chartItem == null)
            {
                return HttpNotFound();
            }
            return View(chartItem);
        }

        // GET: AdminCharts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminCharts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "chartId")] ChartItem chartItem)
        {
            if (ModelState.IsValid)
            {
                db.ChartItems.Add(chartItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chartItem);
        }

        // GET: AdminCharts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChartItem chartItem = db.ChartItems.Find(id);
            if (chartItem == null)
            {
                return HttpNotFound();
            }
            return View(chartItem);
        }

        // POST: AdminCharts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "chartId")] ChartItem chartItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chartItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chartItem);
        }

        // GET: AdminCharts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChartItem chartItem = db.ChartItems.Find(id);
            if (chartItem == null)
            {
                return HttpNotFound();
            }
            return View(chartItem);
        }

        // POST: AdminCharts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChartItem chartItem = db.ChartItems.Find(id);
            db.ChartItems.Remove(chartItem);
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
