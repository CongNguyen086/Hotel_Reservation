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
    public class AdminRegistered_CustomerController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Registered_Customer
        public ActionResult Index()
        {
            return View(db.Registered_Customers.ToList());
        }

        // GET: Registered_Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registered_Customer registered_Customer = db.Registered_Customers.Find(id);
            if (registered_Customer == null)
            {
                return HttpNotFound();
            }
            return View(registered_Customer);
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
