using DBlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlinePizzaRestaurantWebApp.Models
{
    public class DashboardMV
    {
        OnlinePizzaRestaurantDbEntities db = new OnlinePizzaRestaurantDbEntities();
        public DashboardMV()
        {
            ProfileMV = new User_ProfileMV();
        }
        public DashboardMV(int? id)
        {
            ProfileMV = new User_ProfileMV();
            var user = db.UserTables.Find(id);
            ProfileMV.UserID = user.UserID;
            ProfileMV.UserType = user.UserTypeTable.UserType;
            ProfileMV.UserTypeID = user.UserTypeID;
            ProfileMV.UserName = user.UserName;
            ProfileMV.Password = user.Password;
            ProfileMV.FirstName = user.FirstName;
            ProfileMV.LastName = user.LastName;
            ProfileMV.FullName = string.Format("{0} {1}", user.FirstName, user.LastName);
            ProfileMV.ContactNo = user.ContactNo;
            ProfileMV.GenderTitle = user.Gender.GenderTitle;
            ProfileMV.EmailAddress = user.EmailAddress;
            ProfileMV.RegisterationDate = user.RegistrationDate;
            var userpersonaladdress = db.UserAddressTables.Where(u => u.UserID == user.UserID).FirstOrDefault();

            ProfileMV.FullAddress = userpersonaladdress != null ? userpersonaladdress.FullAddress : string.Empty;
            ProfileMV.UserStatus = user.UserStatusTable.UserStatus;
            ProfileMV.UserStatusID = user.UserStatusID;
            ProfileMV.UserStatusChangeDate = user.UserStatusChangeDate;
            if (user.UserDetailTable != null)
            {
                ProfileMV.UserDetailProvideDate = user.UserDetailTable.UserDetailProviderDate;
                ProfileMV.PhotoPath = user.UserDetailTable.PhotoPath;
                ProfileMV.CNIC = user.UserDetailTable.CNIC;
                ProfileMV.EducationLevel = user.UserDetailTable.EducationLevel;
                ProfileMV.ExperenceLevel = user.UserDetailTable.ExperenceLevel;
                ProfileMV.EducationLastDegreePhotoPath = user.UserDetailTable.EducationLastDegreeScanPhotoPath;
                ProfileMV.ExperenceLastPhotoPath = user.UserDetailTable.LastExperenceScanPhotoPath;
            }
            GetUserAddress(user.UserID);
            GetUserOrders(user.UserID);
        }
        public virtual User_ProfileMV ProfileMV { get; set; }

        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public virtual List<UserAddressMV> UserAddress { get; set; }

        public virtual List<CustomerOrdersMV> AllOrders { get; set; }

        public void GetUserAddress(int userid)
        {
            UserAddress = new List<UserAddressMV>();
            foreach (var address in db.UserAddressTables.Where(u => u.VisibleStatusID == 1).ToList())
            {
                UserAddress.Add(new UserAddressMV()
                {
                    UserAddressID = address.UserAddressID,
                    AddressType = address.AddressTypeTable.AddressType,
                    FullAddress = address.FullAddress,
                    VisibleStatus = address.VisibleStatusTable.VisibleStatus,
                });
            }
        }

        public void GetUserOrders(int userid)
        {
            AllOrders = new List<CustomerOrdersMV>();
            var orders = db.v_Orders.Where(u => u.Customer_UserID == userid).OrderByDescending(o => o.OrderID).Take(3).ToList();
            foreach (var item in orders)
            {
                var customerorder = new CustomerOrdersMV();
                customerorder.OrderID = item.OrderID;
                customerorder.Customer_UserID = item.Customer_UserID;
                customerorder.Customer_UserName = item.Customer_UserName;
                customerorder.Order_DateTime = DateTime.Now;
                customerorder.OrderTypeID = item.OrderTypeID;
                customerorder.OrderType = item.OrderTpe;
                customerorder.Customer_FullName = item.Customer_FullName;
                customerorder.Customer_ContactNo = item.Customer_ContactNo;
                customerorder.OrderStatusID = item.OrderStatusID;
                customerorder.OrderStatus = item.OrderStatus;
                customerorder.Description = item.Description;
                customerorder.ProcessBy_UserID = item.ProcessBy_UserID;
                customerorder.ProcessUserName = item.ProcessUserName;
                customerorder.OrderPaymentID = item.OrderPaymentID;
                customerorder.OrderPaymentStatus = item.OrderPaymentStatus;
                customerorder.DeliveryAddressID = item.DeliveryAddressID;
                customerorder.DeliveryAddress = item.DeliveryAddress;
                customerorder.OrderDetails = new List<OrderDetailsMV>();
                foreach (var order_item in db.OrderItemDetailTables.Where(i => i.OrderID == item.OrderID).ToList())
                {

                    var stockitem = db.StockItemTables.Find(order_item.StockItemID);
                    customerorder.OrderDetails.Add(new OrderDetailsMV()
                    {
                        ItemDetailID = order_item.OrderDetailID,
                        StockItemID = order_item.StockItemID,
                        StockItemTitle = stockitem.StockItemTitle,
                        StockItemCategory = stockitem.StockItemCategoryTable.StockItemCategory,
                        ItemPhotoPath = stockitem.ItemPhotoPath,
                        ItemSize = stockitem.ItemSize,
                        UnitPrice = stockitem.UnitPrice,
                        Qty = order_item.Qty,
                        ItemCost = stockitem.UnitPrice * order_item.Qty,
                        ItemType = "item"
                    });
                    customerorder.TotalCost += stockitem.UnitPrice * order_item.Qty;
                }

                foreach (var order_deal in db.OrderDealDetailTables.Where(d => d.OrderID == item.OrderID).ToList())
                {
                    var stockdeal = db.StockDealTables.Find(order_deal.StockDealID);
                    customerorder.OrderDetails.Add(new OrderDetailsMV()
                    {
                        ItemDetailID = order_deal.OrderDealDetailID,
                        StockItemID = order_deal.StockDealID,
                        StockItemTitle = stockdeal.StockDealTitle,
                        StockItemCategory = "Deal",
                        ItemPhotoPath = stockdeal.DealPhoto,
                        ItemSize = "Normal",
                        UnitPrice = stockdeal.DealPrice,
                        Qty = order_deal.Qty,
                        ItemCost = stockdeal.DealPrice * order_deal.Qty,
                        ItemType = "Deal"
                    });
                    customerorder.TotalCost += stockdeal.DealPrice * order_deal.Qty;
                }
                AllOrders.Add(customerorder);
            }
        }
    }
}