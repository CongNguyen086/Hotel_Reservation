using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hotel_Reservation.Models;
using PayPal.Api;

namespace Hotel_Reservation.Controllers
{
    public class CartController : Controller
    {
        private ModelContext db = new ModelContext();
        private string strCart = "giohang";
        // GET: Cart
        public ActionResult Index()
        {
            List<BookingItem> cart = Session[strCart] as List<BookingItem>;
            if(cart != null)
            {
                ViewBag.Subtotal = cart.Select(m => m.itemTotalPrice).Sum();
                ViewBag.Total = ViewBag.Subtotal + 5;
            }
            
            return View(cart);

        }

        public RedirectToRouteResult AddToCart(string typeId)
        {
            if (Session[strCart] == null)
            {
                Session[strCart] = new List<BookingItem>();  
            }

            List<BookingItem> cart = Session[strCart] as List<BookingItem>;
            Room_Catalog rc = db.Room_Catalogs.Find(typeId);
            Room r = db.Rooms.FirstOrDefault(m => (m.typeId == typeId) 
                                                && (m.roomStatus == "Available")
                                                && (m.operationStatus == "Active"));
            Image_Detail img = db.Image_Details.FirstOrDefault(m => m.typeId == typeId);
            var checkIn = (DateTime)Session["CheckIn"];
            var checkOut = (DateTime)Session["CheckOut"];
            //var checkInParse = DateTime.ParseExact(checkIn.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //var checkOutParse = DateTime.ParseExact(checkOut.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var numberOfNight = (checkOut - checkIn).Days;
            decimal discount = 0;
            var itemPromotion = "";
            var promotion = db.Promotions.FirstOrDefault(p => (p.appliedRoomType.Equals(typeId))
                                                                && (p.promotionStatus.Equals("Processing")));
            if(promotion != null)
            {
                discount = promotion.roomDiscount ?? 0;
                itemPromotion = promotion.promotionDescription;
            }
            BookingItem newItem = new BookingItem()
            {
                roomNumber = r.roomNumber,
                image = img.image,
                typeName = rc.typeName,
                guest = rc.numberOfAdults + rc.numberOfChild,
                numberOfAdult = rc.numberOfAdults,
                numberOfChild = rc.numberOfChild,
                unitPrice = rc.price,
                night = numberOfNight,
                extraFee = 0,
                discount = rc.price * discount,
                promotion = itemPromotion,
            };
            
            cart.Add(newItem);

            return RedirectToAction("Details", "Homes", new { id = typeId });
        }

        public RedirectToRouteResult UpdateDate(DateTime checkIn, DateTime checkOut)
        {
            Session["CheckIn"] = checkIn;
            Session["CheckOut"] = checkOut;
            return RedirectToAction("Index");
        }

        public RedirectToRouteResult DeleteItem(int roomNumber)
        {
            List<BookingItem> giohang = Session[strCart] as List<BookingItem>;
            BookingItem item = giohang.FirstOrDefault(m => m.roomNumber == roomNumber);
            if (item != null)
            {
                giohang.Remove(item);
            }
            return RedirectToAction("Index");
        }
    }
}