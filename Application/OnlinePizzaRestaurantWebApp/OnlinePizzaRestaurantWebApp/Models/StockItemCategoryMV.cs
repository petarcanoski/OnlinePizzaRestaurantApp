using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePizzaRestaurantWebApp.Models
{
    public class StockItemCategoryMV
    {
        public int StockItemCategoryID { get; set; }
        public string StockItemCategory { get; set; }
        public string CreatedBy { get; set; }
        public string VisibleStatus { get; set; }
        public int StockMenuCategoryID { get; internal set; }
        public string StockMenuCategory { get; internal set; }
    }
}