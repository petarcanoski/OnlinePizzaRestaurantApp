using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePizzaRestaurantWebApp.Models
{
    public class OrderDetailsMV
    {
        public int ItemDetailID { get; set; }
        public int StockItemID { get; set; }
        public string StockItemTitle { get; set; }
        public string StockItemCategory { get; set; }
        public string ItemPhotoPath { get; set; }
        public string ItemSize { get; set; }
        public double UnitPrice { get; set; }
        public int Qty { get; set; }
        public double ItemCost { get; set; }
        public string ItemType { get; set; }
    }
}