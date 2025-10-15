using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePizzaRestaurantWebApp.Models
{
    public class CustomerOrdersMV
    {
        public int OrderID { get; set; }
        public int Customer_UserID { get; set; }
        public string Customer_UserName { get; set; }
        public System.DateTime Order_DateTime { get; set; }
        public int OrderTypeID { get; set; }
        public string OrderType { get; set; }
        public string Customer_FullName { get; set; }
        public string Customer_ContactNo { get; set; }
        public int OrderStatusID { get; set; }
        public string OrderStatus { get; set; }
        public string Description { get; set; }
        public int ProcessBy_UserID { get; set; }
        public string ProcessUserName { get; set; }
        public int OrderPaymentID { get; set; }
        public string OrderPaymentStatus { get; set; }
        public int DeliveryAddressID { get; set; }
        public string DeliveryAddress { get; set; }
        public double TotalCost { get; set; }

        public List<OrderDetailsMV> OrderDetails { get; set; }
    }
}