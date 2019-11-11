using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel_Reservation.Models;
using PayPal.Api;

namespace Hotel_Reservation.Controllers
{
    public class PayPalsController : Controller
    {
        private ModelContext db = new ModelContext();
        private string strCart = "giohang";
        // PayPal Payment
        private Payment payment;
        // Create ExecutePayment method
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            payment = new Payment()
            {
                id = paymentId
            };
            return payment.Execute(apiContext, paymentExecution);
        }
        // Create payment info
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price
            List<CartItem> cart = Session[strCart] as List<CartItem>;
            foreach (var cartItem in cart)
            {
                List<NameValuePair> supplementary_data = new List<NameValuePair>
                {
                    new NameValuePair{ name = "Adults", value = cartItem.numberOfAdult.ToString() },
                    new NameValuePair{ name = "Children", value = cartItem.numberOfChild.ToString() },
                    new NameValuePair{ name = "Discount", value = cartItem.discount.ToString() }
                };
                //itemList.items.Add(new Item()
                //{
                //    name = cartItem.typeName,
                //    currency = "USD",
                //    price = cartItem.unitPrice.ToString(),
                //    supplementary_data = supplementary_data,
                //    description = cartItem.promotion,
                //});
                itemList.items.Add(new Item()
                {
                    name = cartItem.typeName,
                    quantity = cartItem.night.ToString(),
                    currency = "USD",
                    price = cartItem.unitPrice.ToString(),
                    description = cartItem.promotion,
                    supplementary_data = supplementary_data,
                });
            }

            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                //cancel_url = redirectUrl + "&Cancel=true",
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details
            var details = new Details()
            {
                //fee = cart.Select(m => m.extraFee).Sum().ToString(),
                //shipping_discount = cart.Select(m => m.discount).Sum
                //subtotal = cart.Select(m => m.itemTotalPrice).Sum().ToString(),
                tax = cart.Select(m => m.extraFee).Sum().ToString(),
                shipping_discount = cart.Select(m => m.discount).Sum().ToString(),
                subtotal = cart.Select(m => m.itemTotalPrice).Sum().ToString(),
            };
            //Final amount with details
            var total = Convert.ToDouble(details.tax) + Convert.ToDouble(details.subtotal) - Convert.ToDouble(details.shipping_discount);
            var amount = new Amount()
            {
                currency = "USD",
                total = total.ToString(), // Total must be equal to sum of fee and subtotal.  
                //total = details.subtotal + details.fee,
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Hotel Booking",
                invoice_number = Convert.ToString((new Random()).Next(100000)), //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return payment.Create(apiContext);
        }

        // Create payment method
        public ActionResult PaymentWithPaypal()
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfig.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/PayPals/PaymentWithPaypal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = string.Empty;
                    while (links.MoveNext())
                    {
                        Links link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = link.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("Failure");
                    }
                }
            }
            catch (PayPal.PayPalException ex)
            {
                PaypalLogger.Log("Error: " + ex.Message);
                return View("Failure");
                //if (ex.InnerException is PayPal.ConnectionException)
                //{
                //    Response.Write(((PayPal.ConnectionException)ex.InnerException).Response);
                //}
                //else
                //{
                //    Response.Write(ex.Message);
                //}
            }
            //on successful payment, show success page to user.  
            return RedirectToAction("Create", "Reservations");
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
