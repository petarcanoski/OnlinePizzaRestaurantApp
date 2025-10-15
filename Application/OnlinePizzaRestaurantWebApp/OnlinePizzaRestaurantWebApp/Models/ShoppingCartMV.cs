using DBlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePizzaRestaurantWebApp.Models
{
    public class ShoppingCartMV
    {
        OnlinePizzaRestaurantDbEntities Db = new OnlinePizzaRestaurantDbEntities();
        public ShoppingCartMV(int userid)
        {
            Cart_Items = new List<CartMV>();
            var user = Db.UserTables.Find(userid);
            FirstName = user.FirstName;
            LastName = user.LastName;
            ContactNo = user.ContactNo;
        }

        public ShoppingCartMV()
        {
            Cart_Items = new List<CartMV>();
        }
        public List<CartMV> Cart_Items { get; set; }
        public int UserAddressID { get; set; }
        public int OrderTypeID { get; set; }

        public bool CashOnDelivery { get; set; } = true;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }
    }
}