using DBlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePizzaRestaurantWebApp.Models
{
    public class CRU_StockMenuCategoryMV
    {

        OnlinePizzaRestaurantDbEntities db = new OnlinePizzaRestaurantDbEntities();

        public CRU_StockMenuCategoryMV()
        {
            GetData();
        }
        public CRU_StockMenuCategoryMV(int? id)
        {
            GetData();
            var editStockMenuCategory = db.StockMenuCategoryTables.Find(id);
            if (editStockMenuCategory != null)
            {
                StockMenuCategoryID = editStockMenuCategory.StockMenuCategoryID;
                StockMenuCategory = editStockMenuCategory.StockMenuCategory;
                CreatedBy_UserID = editStockMenuCategory.CreatedBy_UserID;
            }
            else
            {
                StockMenuCategoryID = 0;
                StockMenuCategory = string.Empty;
                CreatedBy_UserID = 0;
                    
            }
        }

        public int StockMenuCategoryID { get; set; }
        public int CreatedBy_UserID { get; set; }

        public string StockMenuCategory { get; set; }
        public virtual List<StockItemCategoryMV> Lists { get; set; }

        private void GetData()
        {
            Lists = new List<StockItemCategoryMV>();
            foreach (var stockMenuCategory in db.StockMenuCategoryTables.ToList())
            {
                var username = db.UserTables.Find(stockMenuCategory.CreatedBy_UserID).UserName;
                Lists.Add(new StockItemCategoryMV()
                {
                    StockMenuCategoryID = stockMenuCategory.StockMenuCategoryID,
                    CreatedBy = username,
                    StockMenuCategory = stockMenuCategory.StockMenuCategory
                });
            }
        }
    }
}