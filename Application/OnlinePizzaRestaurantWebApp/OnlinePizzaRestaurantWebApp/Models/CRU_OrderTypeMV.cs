using DBlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace OnlinePizzaRestaurantWebApp.Models
{
    public class CRU_OrderTypeMV
    {
        OnlinePizzaRestaurantDbEntities db = new OnlinePizzaRestaurantDbEntities();

        public CRU_OrderTypeMV()
        {
            GetData();
        }

        public CRU_OrderTypeMV(int? id)
        {
            GetData();
            var editOrderType = db.OrderTypeTables.Find(id);
            if (editOrderType != null)
            {
                OrderTypeID = editOrderType.OrderTypeID;
                OrderTpe = editOrderType.OrderTpe;
            }
            else
            {
                OrderTypeID = 0;
                OrderTpe = string.Empty;
            }
        }

        public int OrderTypeID { get; set; }
        [Required(ErrorMessage = "Order Type is required")]

        public string OrderTpe { get; set; }
        public virtual List<OrderTypeMV> Lists{ get; set; }
        
        private void GetData()
        {
            Lists = new List<OrderTypeMV>();

            foreach (var orderType in db.OrderTypeTables.ToList())
            {
                Lists.Add(new OrderTypeMV()
                {
                    OrderTypeID = orderType.OrderTypeID,
                    OrderType = orderType.OrderTpe
                });
            }
        }
    }
}