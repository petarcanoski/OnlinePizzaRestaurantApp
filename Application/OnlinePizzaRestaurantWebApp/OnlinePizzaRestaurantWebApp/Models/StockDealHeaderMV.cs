using DBlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePizzaRestaurantWebApp.Models
{
    public class StockDealHeaderMV
    {
        OnlinePizzaRestaurantDbEntities db = new OnlinePizzaRestaurantDbEntities();
        public StockDealHeaderMV(int? dealid)
        {
            GetDealItems(dealid);
        }
        public int StockDealID { get; set; }
        public string StockDealTitle { get; set; }
        public double DealPrice { get; set; }
        public string DealPhoto { get; set; }
        public string VisibleStatus { get; set; }
        public System.DateTime StockDealStartDate { get; set; }
        public double Discount { get; set; }
        public System.DateTime StockDealEndDate { get; set; }
        public System.DateTime StockDealRegisterDate { get; set; }
        public string DealItems { get; set; }

        public void GetDealItems(int? dealid)
        {
            foreach (var item in db.StockDealDetailTables.Where(d => d.StockDealID == dealid).ToList())
            {
                DealItems = DealItems + " | " + item.StockItemTable.StockItemTitle + " (Qty : " + item.Quantity + ")";
            }
        }
    }
}