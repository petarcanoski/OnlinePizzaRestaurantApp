using DBlayer;
using OnlinePizzaRestaurantWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlinePizzaRestaurantWebApp.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        OnlinePizzaRestaurantDbEntities Db = new OnlinePizzaRestaurantDbEntities();
        public ActionResult Cart_AddItem(int? itemid, int? qty, string itemtype, string return_url)
        {
            bool result = false;
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                result = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            int userid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);

            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    var cart = Db.CartTables.Where(u => u.UserID == userid).FirstOrDefault();
                    if (cart == null)
                    {
                        var create_cart = new CartTable()
                        {
                            UserID = userid
                        };
                        Db.CartTables.Add(create_cart);
                        Db.SaveChanges();
                        cart = Db.CartTables.Where(u => u.UserID == userid).FirstOrDefault();
                    }

                    if (itemtype == "item")
                    {
                        var item = Db.CartItemDetailTables.Where(i => i.StockItemID == itemid && i.CartID == cart.CartID).FirstOrDefault();
                        if (item != null)
                        {
                            item.Qty = item.Qty + 1;
                            Db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            Db.SaveChanges();
                        }
                        else
                        {
                            var create_item = new CartItemDetailTable()
                            {
                                CartID = cart.CartID,
                                StockItemID = Convert.ToInt32(itemid),
                                Qty = Convert.ToInt32(qty)
                            };
                            Db.CartItemDetailTables.Add(create_item);
                            Db.SaveChanges();
                        }
                    }
                    else
                    {
                        var deal = Db.CartDealTables.Where(d => d.StockDealID == itemid && d.CartID == cart.CartID).FirstOrDefault();
                        if (deal != null)
                        {
                            deal.Qty = deal.Qty + 1;
                            Db.Entry(deal).State = System.Data.Entity.EntityState.Modified;
                            Db.SaveChanges();
                        }
                        else
                        {
                            var create_deal = new CartDealTable()
                            {
                                CartID = cart.CartID,
                                Qty = Convert.ToInt32(qty),
                                StockDealID = Convert.ToInt32(itemid)
                            };
                            Db.CartDealTables.Add(create_deal);
                            Db.SaveChanges();
                        }
                    }
                    result = true;
                    transaction.Commit();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    result = false;
                    transaction.Rollback();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult ViewCart()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            int userid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);

            var shoppingcart = Cart(userid);

            ViewBag.UserAddressID = new SelectList(Db.UserAddressTables.Where(ua => ua.UserID == userid).ToList(), "UserAddressID", "FullAddress", "0");
            ViewBag.OrderTypeID = new SelectList(Db.OrderTypeTables.ToList(), "OrderTypeID", "OrderTpe", "0");
            return View(shoppingcart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewCart(ShoppingCartMV obj)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            int userid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);

            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    var cart = Db.CartTables.Where(u => u.UserID == userid).FirstOrDefault();
                    int cartid = (cart != null ? cart.CartID : 0);
                    if (cartid > 0)
                    {
                        var order_Header = new OrderTable();
                        order_Header.OrderBy_UserID = userid;
                        order_Header.OrderDateTime = DateTime.Now;
                        order_Header.OrderTypeID = obj.OrderTypeID;
                        order_Header.DeliveryAddres_UserAddressID = obj.UserAddressID;
                        order_Header.ProcessBy_UserID = userid;
                        order_Header.OrderReceivedBy_ContactNo = obj.ContactNo;
                        order_Header.OrderReceivedBy_FullName = obj.FirstName + " " + obj.LastName;
                        order_Header.OrderStatusID = 1; // Pending 
                        order_Header.Description = "Please Wait...";
                        order_Header.OrderPaymentID = 1;
                        Db.OrderTables.Add(order_Header);
                        Db.SaveChanges();

                        var cartItems = Db.CartItemDetailTables.Where(i => i.CartID == cartid);
                        foreach (var cart_item in cartItems.ToList())
                        {
                            var item = new OrderItemDetailTable()
                            {
                                OrderID = order_Header.OrderID,
                                StockItemID = cart_item.StockItemID,
                                UnitPrice = cart_item.StockItemTable.UnitPrice,
                                Qty = cart_item.Qty,
                                DiscountID = 1,
                                DiscountAmount = 0
                            };
                            Db.OrderItemDetailTables.Add(item);
                            Db.SaveChanges();
                        }
                        Db.CartItemDetailTables.RemoveRange(cartItems);
                        Db.SaveChanges();

                        var deals = Db.CartDealTables.Where(d => d.CartID == cartid);
                        foreach (var cart_deal in deals.ToList())
                        {
                            var deal = new OrderDealDetailTable()
                            {
                                OrderID = order_Header.OrderID,
                                StockDealID = cart_deal.StockDealID,
                                DealPrice = cart_deal.StockDealTable.DealPrice,
                                Qty = cart_deal.Qty,
                            };
                            Db.OrderDealDetailTables.Add(deal);
                            Db.SaveChanges();
                            Db.Entry(cart_deal).State = System.Data.Entity.EntityState.Deleted;
                            Db.SaveChanges();
                        }

                        Db.CartDealTables.RemoveRange(deals);
                        Db.SaveChanges();

                        Db.Entry(cart).State = System.Data.Entity.EntityState.Deleted;
                        Db.SaveChanges();

                        transaction.Commit();
                        return RedirectToAction("Dashboard", "User"); // User/Dashboard
                    }

                    transaction.Rollback();
                    ModelState.AddModelError("", "Some Issue is Occure! Please Re-login and Try Again.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "Some Issue is Occure! Please Re-login and Try Again.");
                }
            }
            var shoppingcart = Cart(userid);
            ViewBag.UserAddressID = new SelectList(Db.UserAddressTables.Where(ua => ua.UserID == userid).ToList(), "UserAddressID", "FullAddress", obj.UserAddressID);
            ViewBag.OrderTypeID = new SelectList(Db.OrderTypeTables.ToList(), "OrderTypeID", "OrderTpe", obj.OrderTypeID);
            return View(shoppingcart);
        }

        public ActionResult Add_Qty(int? cartitemid, int? qty, string itemtype)
        {
            bool result = false;
            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    if (itemtype == "item")
                    {
                        var item = Db.CartItemDetailTables.Where(i => i.CartItemID == cartitemid).FirstOrDefault();
                        if (item != null)
                        {
                            item.Qty = item.Qty + 1;
                            Db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            Db.SaveChanges();
                        }
                    }
                    else
                    {
                        var deal = Db.CartDealTables.Where(d => d.CartDealID == cartitemid).FirstOrDefault();
                        if (deal != null)
                        {
                            deal.Qty = deal.Qty + 1;
                            Db.Entry(deal).State = System.Data.Entity.EntityState.Modified;
                            Db.SaveChanges();
                        }
                    }
                    result = true;
                    transaction.Commit();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    result = false;
                    transaction.Rollback();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult Minus_Qty(int? cartitemid, int? qty, string itemtype)
        {
            bool result = false;
            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    if (itemtype == "item")
                    {
                        var item = Db.CartItemDetailTables.Where(i => i.CartItemID == cartitemid).FirstOrDefault();
                        if (item != null)
                        {
                            if (item.Qty == 1)
                            {
                                Db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                                Db.SaveChanges();
                            }
                            else
                            {
                                item.Qty = item.Qty - 1;
                                Db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                                Db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var deal = Db.CartDealTables.Where(d => d.CartDealID == cartitemid).FirstOrDefault();
                        if (deal != null)
                        {
                            if (deal.Qty == 1)
                            {
                                Db.Entry(deal).State = System.Data.Entity.EntityState.Deleted;
                                Db.SaveChanges();
                            }
                            else
                            {
                                deal.Qty = deal.Qty - 1;
                                Db.Entry(deal).State = System.Data.Entity.EntityState.Modified;
                                Db.SaveChanges();
                            }
                        }
                    }
                    result = true;
                    transaction.Commit();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    result = false;
                    transaction.Rollback();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult Delete_Item(int? cartitemid, string itemtype)
        {
            bool result = false;
            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    if (itemtype == "item")
                    {
                        var item = Db.CartItemDetailTables.Where(i => i.CartItemID == cartitemid).FirstOrDefault();
                        if (item != null)
                        {
                            if (item.Qty == 1)
                            {
                                Db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                                Db.SaveChanges();
                            }
                            else
                            {
                                Db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                                Db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var deal = Db.CartDealTables.Where(d => d.CartDealID == cartitemid).FirstOrDefault();
                        if (deal != null)
                        {
                            if (deal.Qty == 1)
                            {
                                Db.Entry(deal).State = System.Data.Entity.EntityState.Deleted;
                                Db.SaveChanges();
                            }
                            else
                            {
                                deal.Qty = deal.Qty - 1;
                                Db.Entry(deal).State = System.Data.Entity.EntityState.Deleted;
                                Db.SaveChanges();
                            }
                        }
                    }
                    result = true;
                    transaction.Commit();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    result = false;
                    transaction.Rollback();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ShoppingCartMV Cart(int userid)
        {
            var cart = Db.CartTables.Where(u => u.UserID == userid).FirstOrDefault();
            var shoppingcart = new ShoppingCartMV(userid);
            var user = Db.UserTables.Find(userid);

            int cartid = (cart != null ? cart.CartID : 0);
            foreach (var cart_item in Db.CartItemDetailTables.Where(i => i.CartID == cartid).ToList())
            {
                var stockitem = Db.StockItemTables.Find(cart_item.StockItemID);
                shoppingcart.Cart_Items.Add(new CartMV()
                {
                    CartID = cart.CartID,
                    CartItemID = cart_item.CartItemID,
                    StockItemID = cart_item.StockItemID,
                    StockItemTitle = stockitem.StockItemTitle,
                    StockItemCategory = stockitem.StockItemCategoryTable.StockItemCategory,
                    ItemPhotoPath = stockitem.ItemPhotoPath,
                    ItemSize = stockitem.ItemSize,
                    UnitPrice = stockitem.UnitPrice,
                    Qty = cart_item.Qty,
                    ItemCost = stockitem.UnitPrice * cart_item.Qty,
                    ItemType = "item"
                });
            }

            foreach (var cart_deal in Db.CartDealTables.Where(d => d.CartID == cartid).ToList())
            {
                var stockdeal = Db.StockDealTables.Find(cart_deal.StockDealID);
                shoppingcart.Cart_Items.Add(new CartMV()
                {
                    CartID = cart.CartID,
                    CartItemID = cart_deal.CartDealID,
                    StockItemID = cart_deal.StockDealID,
                    StockItemTitle = stockdeal.StockDealTitle,
                    StockItemCategory = "Deal",
                    ItemPhotoPath = stockdeal.DealPhoto,
                    ItemSize = "Normal",
                    UnitPrice = stockdeal.DealPrice,
                    Qty = cart_deal.Qty,
                    ItemCost = stockdeal.DealPrice * cart_deal.Qty,
                    ItemType = "Deal"
                });
            }
            return shoppingcart;
        }

        public ActionResult Cancel_Order(int? orderid)
        {
            bool result = false;
            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    var item = Db.OrderTables.Where(o => o.OrderID == orderid).FirstOrDefault();
                    if (item != null)
                    {
                        item.OrderStatusID = 6;
                        Db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        Db.SaveChanges();
                    }
                    result = true;
                    transaction.Commit();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    result = false;
                    transaction.Rollback();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult AllOrders()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            int userid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);

            var userorder = new List<CustomerOrdersMV>();
            var orders = Db.v_Orders.Where(u => u.Customer_UserID == userid).ToList();
            foreach (var item in orders)
            {
                var customerorder = new CustomerOrdersMV();
                customerorder.OrderID = item.OrderID;
                customerorder.Customer_UserID = item.Customer_UserID;
                customerorder.Customer_UserName = item.Customer_UserName;
                customerorder.Order_DateTime = item.Order_DateTime;
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
                foreach (var order_item in Db.OrderItemDetailTables.Where(i => i.OrderID == item.OrderID).ToList())
                {

                    var stockitem = Db.StockItemTables.Find(order_item.StockItemID);
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

                foreach (var order_deal in Db.OrderDealDetailTables.Where(d => d.OrderID == item.OrderID).ToList())
                {
                    var stockdeal = Db.StockDealTables.Find(order_deal.StockDealID);
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
                userorder.Add(customerorder);
            }
            return View(userorder);
        }

        public ActionResult CustomerOrders()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            int userid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            var date = DateTime.Now.AddDays(-2);
            var userorder = new List<CustomerOrdersMV>();
            var orders = Db.v_Orders.Where(u => u.Order_DateTime >= date).ToList();
            foreach (var item in orders)
            {
                var customerorder = new CustomerOrdersMV();
                customerorder.OrderID = item.OrderID;
                customerorder.Customer_UserID = item.Customer_UserID;
                customerorder.Customer_UserName = item.Customer_UserName;
                customerorder.Order_DateTime = item.Order_DateTime;
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
                foreach (var order_item in Db.OrderItemDetailTables.Where(i => i.OrderID == item.OrderID).ToList())
                {

                    var stockitem = Db.StockItemTables.Find(order_item.StockItemID);
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

                foreach (var order_deal in Db.OrderDealDetailTables.Where(d => d.OrderID == item.OrderID).ToList())
                {
                    var stockdeal = Db.StockDealTables.Find(order_deal.StockDealID);
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
                userorder.Add(customerorder);
            }
            return View(userorder);
        }

        public ActionResult Update_Order(int? orderid, int? statusid)
        {
            bool result = false;
            using (var transaction = Db.Database.BeginTransaction())
            {
                try
                {
                    var item = Db.OrderTables.Where(o => o.OrderID == orderid).FirstOrDefault();
                    if (item != null)
                    {
                        item.OrderStatusID = (int)statusid;
                        Db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                        Db.SaveChanges();
                    }
                    result = true;
                    transaction.Commit();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    result = false;
                    transaction.Rollback();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}