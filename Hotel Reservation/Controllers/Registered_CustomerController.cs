﻿using System;
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
    public class Registered_CustomerController : Controller
    {
        private ModelContext db = new ModelContext();

        public ActionResult LoginCustomer()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginCustomer(Registered_Customer loginCustomer)
        {
            try
            {
                var acc = db.Registered_Customers.SingleOrDefault(a => a.customerId.Equals(loginCustomer.customerId));
                if (acc == null)
                {
                    ModelState.AddModelError("", "Invalid Customer ID");
                }
                else
                {
                    if (acc.password.Equals(loginCustomer.password))
                    {
                        Session["customerId"] = acc.customerId;
                        Session["customerName"] = acc.customerName;
                        Session["phone"] = acc.phone;
                        Session["email"] = acc.email;
                        return RedirectToAction("Index", "Homes");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Password");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Customer is not found!");
            }
            return View();
        }

        public ActionResult LogoutCustomer()
        {
            Session["customerId"] = null;
            Session["customerName"] = null;
            Session["phone"] = null;
            Session["email"] = null;
            Session["CheckIn"] = null;
            Session["CheckOut"] = null;
            return RedirectToAction("LoginCustomer", "Registered_Customer");
        }

        // GET: Registered_Customer
        public ActionResult Index()
        {
            return View(db.Registered_Customers.ToList());
        }

        public ActionResult ReservationHistory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registered_Customer cus = db.Registered_Customers.Find(id);
            var reserv = db.Reservations.Where(m => m.customerId == id);

            ViewBag.CustomerName = cus.customerName;
            ViewBag.Phone = cus.phone;

            return View(reserv.ToList());
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
