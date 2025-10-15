using DBlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePizzaRestaurantWebApp.Models
{
    public class HomeMV
    {
        OnlinePizzaRestaurantDbEntities db = new OnlinePizzaRestaurantDbEntities();
        public HomeMV()
        {
            GetMenuCategories();
            GetPopularDishes();
        }
        public virtual List<MenuCategoryMV> Menu { get; set; }
        public void GetMenuCategories()
        {
            Menu = new List<MenuCategoryMV>();
            foreach (var item in db.StockMenuCategoryTables.ToList())
            {
                Menu.Add(new MenuCategoryMV(item.StockMenuCategoryID)
                {
                    MenuCategory = item.StockMenuCategory
                });
            }
        }

        public List<StockItemMV> PopularDishes { get; set; }
        public void GetPopularDishes()
        {
            PopularDishes = new List<StockItemMV>();
            foreach (var item in db.StockItemTables.Where(c => c.StockItemCategoryID == 3 && c.VisibleStatusID == 1).ToList()) 
            {
                var visiblestatus = db.VisibleStatusTables.Find(item.VisibleStatusID).VisibleStatus;
                var createdby = db.UserTables.Find(item.CreatedBy_UserID).UserName;
                PopularDishes.Add(new StockItemMV()
                {
                    StockItemID = item.StockItemID,
                    StockItemCategory = item.StockItemCategoryTable.StockItemCategory,
                    ItemPhotoPath = item.ItemPhotoPath,
                    StockItemTitle = item.StockItemTitle,
                    ItemSize = item.ItemSize,
                    UnitPrice = item.UnitPrice,
                    RegisterDate = item.RegisterDate,
                    VisibleStatus = visiblestatus,
                    CreatedBy = createdby,
                    OrderType = item.OrderTypeTable.OrderTpe,
                });
            }
        }
    }
}