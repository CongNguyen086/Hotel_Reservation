using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hotel_Reservation.Models
{
    public class BookingItem
    {
        [Key]
        public string typeId { get; set; }
        public string image { get; set; }
        public string typeName { get; set; }
        public decimal unitPrice { get; set; }
        public int night { get; set; }
        public decimal extraFee { get; set; }
        public decimal discount { get; set; }
        public string promotion { get; set; }
        public decimal itemTotalPrice
        {
            get
            {
                return unitPrice * night; 
            }
            set
            {
                itemTotalPrice = value;
            }
        } 
    }

    public class TempBooking
    {
        [Key]
        public string typeId { get; set; }
        public string typeName { get; set; }
        public int numberOfRoom { get; set; }
    }

    public class BookingViewModel
    {
        [Column(TypeName = "date")]
        [Display(Name = "Checkin Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime checkIn { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Checkout Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime checkOut { get; set; }

        public int numberOfAdult { get; set; }
        public int numberOfChild { get; set; }
        public List<TempBooking> bookingItems { get; set; }
    }
}