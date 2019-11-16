using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hotel_Reservation.Models
{
    public class RoomCatalog_Promotion
    {
        [Key]
        public string typeId { get; set; }
        public int numberOfAvailableRoom { get; set; } 
        public string typeName { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        [Range(1, 100000, ErrorMessage = "Price must be between 1 and 100000")]
        public decimal price { get; set; }

        public string description { get; set; }
        public Nullable<int> quantityOfRooms { get; set; }
        public int numberOfAdults { get; set; }
        public Nullable<int> numberOfChild { get; set; }
        public decimal extraFee { get; set; }
        public string catalogStatus { get; set; }
        public List<Image_Detail> image { get; set; }
        //public string promotionId { get; set; }
        public string promotionDescription { get; set; }
        public string appliedRoomType { get; set; }
        public Nullable<decimal> roomDiscount { get; set; }
    }
}