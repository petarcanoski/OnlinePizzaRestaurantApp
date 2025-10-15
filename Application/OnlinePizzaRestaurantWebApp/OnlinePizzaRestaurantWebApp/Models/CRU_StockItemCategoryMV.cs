using DBlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePizzaRestaurantWebApp.Models
{
    public class CRU_StockItemCategoryMV
    {
        OnlinePizzaRestaurantDbEntities db = new OnlinePizzaRestaurantDbEntities();

        public CRU_StockItemCategoryMV()
        {
            GetAllCategories();
        }
        public CRU_StockItemCategoryMV(int? id)
        {
            GetAllCategories();
            var editCategory = db.StockItemCategoryTables.Find(id);
            if (editCategory != null)
            {
                StockItemCategoryID = editCategory.StockItemCategoryID;
                StockItemCategory = editCategory.StockItemCategory;
                VisibleStatusID = editCategory.VisibleStatusID;
            }
            else
            {
                StockItemCategoryID = 0;
                StockItemCategory = string.Empty;
                VisibleStatusID = 0;
            }
        }

        public int StockItemCategoryID { get; set; }
        public string StockItemCategory { get; set; }
        public int CreatedBy_UserID { get; set; }
        public int VisibleStatusID { get; set; }
        public virtual List<StockItemCategoryMV> Lists { get; set; }

        public void GetAllCategories()
        {
            Lists = new List<StockItemCategoryMV>();
            foreach (var category in db.StockItemCategoryTables.ToList())
            {
                var username = db.UserTables.Find(category.CreatedBy_UserID).UserName;
                var status = db.VisibleStatusTables.Find(category.VisibleStatusID).VisibleStatus;
                Lists.Add(new StockItemCategoryMV()
                {
                    StockItemCategoryID = category.StockItemCategoryID,
                    StockItemCategory = category.StockItemCategory,
                    CreatedBy = username,
                    VisibleStatus = status
                });
            }
        }
    }
}