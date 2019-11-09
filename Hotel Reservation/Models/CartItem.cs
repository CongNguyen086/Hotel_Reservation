using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hotel_Reservation.Models
{
    public class CartItem
    {
        [Key]
        public int roomNumber { get; set; }
        public string image { get; set; }
        public string typeName { get; set; }
        public int guest { get; set; }
        public int numberOfAdult { get; set; }
        public int numberOfChild { get; set; }
        public decimal unitPrice { get; set; }
        public decimal extraFee { get; set; }
        public decimal discount { get; set; }
        public string promotion { get; set; }
        public decimal itemTotalPrice
        {
            get
            {
                return unitPrice * (1 - discount); 
            }
            set
            {
                itemTotalPrice = value;
            }
        } 
    }
}