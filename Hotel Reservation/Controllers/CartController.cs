using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hotel_Reservation.Models;

namespace Hotel_Reservation.Controllers
{
    public class CartController : Controller
    {
        private ModelContext db = new ModelContext();
        // GET: Cart
        public ActionResult Index()
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            if(giohang != null)
            {
                ViewBag.Subtotal = giohang.Select(m => m.totalPrice).Sum();
                ViewBag.Total = ViewBag.Subtotal + 5;
            }
            
            return View(giohang);

        }

        public RedirectToRouteResult AddToCart(string typeId)
        {
            if (Session["giohang"] == null)
            {
                Session["giohang"] = new List<CartItem>();  
            }

            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            Room_Catalog rc = db.Room_Catalogs.Find(typeId);
            Room r = db.Rooms.Where(m => m.roomStatus == "Available").Where(m => m.operationStatus == "Active").FirstOrDefault(m => m.typeId == typeId);
            Image_Detail img = db.Image_Details.FirstOrDefault(m => m.typeId == typeId);

            CartItem newItem = new CartItem()
            {
                roomNumber = r.roomNumber,
                image = img.image,
                typeName = rc.typeName,
                traveler = rc.numberOfAdults,
                unitPrice = rc.price,
                extraFee = 0,
            };
            
            giohang.Add(newItem);

            return RedirectToAction("Details", "Homes", new { id = typeId });
        }

        public RedirectToRouteResult UpdateDate(DateTime date1, DateTime date2)
        {
            Session["CheckIn"] = Convert.ToDateTime(date1);
            Session["CheckOut"] = Convert.ToDateTime(date2);
            return RedirectToAction("Index");
        }

        public RedirectToRouteResult DeleteItem(int roomNumber)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem item = giohang.FirstOrDefault(m => m.roomNumber == roomNumber);
            if (item != null)
            {
                giohang.Remove(item);
            }
            return RedirectToAction("Index");
        }
    }
}