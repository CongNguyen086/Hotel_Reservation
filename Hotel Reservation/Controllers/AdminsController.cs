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
    public class AdminsController : Controller
    {
        private ModelContext db = new ModelContext();

        public ActionResult LoginAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginAdmin(Admin loginAdmin)
        {
            try
            {
                var acc = db.Admins.SingleOrDefault(a => a.adminId.Equals(loginAdmin.adminId));
                Session["adminId"] = acc.adminId;
                if (acc == null)
                {
                    ModelState.AddModelError("", "Admin is not found!");
                }
                else
                {
                    if (acc.adminPass.Equals(loginAdmin.adminPass))
                    {
                        return RedirectToAction("Index", "Room_Catalog");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Password");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Admin is not found!");
            }
            return View();
        }

        public ActionResult LogoutAdmin()
        {
            Session["adminId"] = null;
            return RedirectToAction("LoginCustomer", "Registered_Customer");
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
