using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel_Reservation.Models
{
    public class ChartItem
    {
        [Key]
        public int chartId { get; set; }
    }
}