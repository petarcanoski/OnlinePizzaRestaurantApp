using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlinePizzaRestaurantWebApp.Models
{
    public class OrderTypeMV
    {
        public int OrderTypeID { get; set; }
        [Required(ErrorMessage = "Order Type is required")]
        public string OrderType { get; set; }
    }
}