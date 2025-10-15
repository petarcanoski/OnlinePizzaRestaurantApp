using DBlayer;
using OnlinePizzaRestaurantWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlinePizzaRestaurantWebApp.Controllers
{
    public class StockController : Controller
    {
        // GET: Stock

        OnlinePizzaRestaurantDbEntities Db = new OnlinePizzaRestaurantDbEntities();

        public ActionResult StockItemCategory(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            var stockcategories = new CRU_StockItemCategoryMV(id);
            ViewBag.VisibleStatusID = new SelectList(Db.VisibleStatusTables.ToList(), "VisibleStatusID", "VisibleStatus", stockcategories.VisibleStatusID);
            return View(stockcategories);
        }

        [HttpPost]
        public ActionResult StockItemCategory(CRU_StockItemCategoryMV stockcategory)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            stockcategory.CreatedBy_UserID = userid;
            if (ModelState.IsValid)
            {
                if (stockcategory.StockItemCategoryID == 0)
                {
                    var checkexist = Db.StockItemCategoryTables.Where(x => x.StockItemCategory.Trim().ToUpper() == stockcategory.StockItemCategory.Trim().ToUpper()).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var newcategory = new StockItemCategoryTable();
                        newcategory.StockItemCategory = stockcategory.StockItemCategory.Trim();
                        newcategory.VisibleStatusID = stockcategory.VisibleStatusID;
                        newcategory.CreatedBy_UserID = userid;
                        Db.StockItemCategoryTables.Add(newcategory);
                        Db.SaveChanges();
                        return RedirectToAction("StockItemCategory", "Stock", new { id = 0 });
                    }
                    else
                    {
                        ModelState.AddModelError("StockItemCategory", "Stock Item Category already registered.");
                    }
                }
                else
                {
                    var checkexist = Db.StockItemCategoryTables.Where(x => x.StockItemCategory.Trim().ToUpper() == stockcategory.StockItemCategory.Trim().ToUpper() && x.StockItemCategoryID != stockcategory.StockItemCategoryID).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var editcategory = Db.StockItemCategoryTables.Find(stockcategory.StockItemCategoryID);
                        editcategory.StockItemCategory = stockcategory.StockItemCategory.Trim();
                        editcategory.VisibleStatusID = stockcategory.VisibleStatusID;
                        editcategory.CreatedBy_UserID = userid;
                        Db.Entry(editcategory).State = System.Data.Entity.EntityState.Modified;
                        Db.SaveChanges();
                        return RedirectToAction("StockItemCategory", "Stock", new { id = 0 });
                    }
                    else
                    {
                        ModelState.AddModelError("StockItemCategory", "Stock Item Category already registered.");
                    }
                }
            }
            ViewBag.VisibleStatusID = new SelectList(Db.VisibleStatusTables.ToList(), "VisibleStatusID", "VisibleStatus", stockcategory.VisibleStatusID);
            return View(stockcategory);
        }


        public ActionResult OrderType(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            var ordertypes = new CRU_OrderTypeMV(id);
            return View(ordertypes);
        }

        [HttpPost]
        public ActionResult OrderType(CRU_OrderTypeMV ordertype)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                if (ordertype.OrderTypeID == 0)
                {
                    var checkexist = Db.OrderTypeTables.Where(x => x.OrderTpe.Trim().ToUpper() == ordertype.OrderTpe.Trim().ToUpper()).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var newordertype = new OrderTypeTable();
                        newordertype.OrderTpe = ordertype.OrderTpe.Trim();
                        Db.OrderTypeTables.Add(newordertype);
                        Db.SaveChanges();
                        return RedirectToAction("OrderType", "Stock", new { id = 0 });
                    }
                    else
                    {
                        ModelState.AddModelError("OrderTpe", "Order Type already registered.");
                    }
                }
                else
                {
                    var checkexist = Db.OrderTypeTables.Where(x => x.OrderTpe.Trim().ToUpper() == ordertype.OrderTpe.Trim().ToUpper() && x.OrderTypeID != ordertype.OrderTypeID).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var editordertype = Db.OrderTypeTables.Find(ordertype.OrderTypeID);
                        editordertype.OrderTpe = ordertype.OrderTpe.Trim();
                        Db.Entry(editordertype).State = System.Data.Entity.EntityState.Modified;
                        Db.SaveChanges();
                        return RedirectToAction("OrderType", "Stock", new { id = 0 });
                    }
                    else
                    {
                        ModelState.AddModelError("OrderTpe", "Order Type already registered.");
                    }
                }
            }
            return View(ordertype);
        }

        public ActionResult StockMenuCategory(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            var menucategories = new CRU_StockMenuCategoryMV(id);

            return View(menucategories);
        }

        [HttpPost]
        public ActionResult StockMenuCategory(CRU_StockMenuCategoryMV menucategory)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            menucategory.CreatedBy_UserID = userid;
            if (ModelState.IsValid)
            {
                if (menucategory.StockMenuCategoryID == 0)
                {
                    var checkexist = Db.StockMenuCategoryTables.Where(x => x.StockMenuCategory.Trim().ToUpper() == menucategory.StockMenuCategory.Trim().ToUpper()).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var newcategory = new StockMenuCategoryTable();
                        newcategory.StockMenuCategory = menucategory.StockMenuCategory.Trim();
                        newcategory.CreatedBy_UserID = userid;
                        Db.StockMenuCategoryTables.Add(newcategory);
                        Db.SaveChanges();
                        return RedirectToAction("StockMenuCategory", "Stock", new { id = 0 });
                    }
                    else
                    {
                        ModelState.AddModelError("StockMenuCategory", "Stock Menu Category already registered.");
                    }
                }
                else
                {
                    var checkexist = Db.StockMenuCategoryTables.Where(x => x.StockMenuCategory.Trim().ToUpper() == menucategory.StockMenuCategory.Trim().ToUpper() && x.StockMenuCategoryID != menucategory.StockMenuCategoryID).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var editcategory = Db.StockMenuCategoryTables.Find(menucategory.StockMenuCategoryID);
                        editcategory.StockMenuCategory = menucategory.StockMenuCategory.Trim();
                        editcategory.CreatedBy_UserID = userid;
                        Db.Entry(editcategory).State = System.Data.Entity.EntityState.Modified;
                        Db.SaveChanges();
                        return RedirectToAction("StockMenuCategory", "Stock", new { id = 0 });
                    }
                    else
                    {
                        ModelState.AddModelError("StockMenuCategory", "Stock Menu Category already registered.");
                    }
                }
            }
            return View(menucategory);
        }

        public ActionResult StockItem(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            var stockitem = new CRU_StockItemMV(id);
            ViewBag.StockItemCategoryID = new SelectList(Db.StockItemCategoryTables.ToList(), "StockItemCategoryID", "StockItemCategory", stockitem.StockItemCategoryID);
            ViewBag.VisibleStatusID = new SelectList(Db.VisibleStatusTables.ToList(), "VisibleStatusID", "VisibleStatus", stockitem.VisibleStatusID);
            ViewBag.OrderTypeID = new SelectList(Db.OrderTypeTables.ToList(), "OrderTypeID", "OrderTpe", stockitem.OrderTypeID);

            return View(stockitem);
        }

        [HttpPost]
        public ActionResult StockItem(CRU_StockItemMV stockitem)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            stockitem.CreatedBy_UserID = userid;
            stockitem.RegisterDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (stockitem.StockItemID == 0)
                {
                    var checkexist = Db.StockItemTables.Where(x => x.StockItemTitle.Trim().ToUpper() == stockitem.StockItemTitle.Trim().ToUpper()
                    && x.StockItemCategoryID == stockitem.StockItemCategoryID
                    && x.OrderTypeID == stockitem.OrderTypeID
                    && x.ItemSize == stockitem.ItemSize).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var newitem = new StockItemTable();
                        newitem.StockItemCategoryID = stockitem.StockItemCategoryID;
                        newitem.ItemPhotoPath = "Content/ItemPhoto/default.png";
                        newitem.StockItemTitle = stockitem.StockItemTitle.Trim();
                        newitem.ItemSize = stockitem.ItemSize.Trim();
                        newitem.UnitPrice = stockitem.UnitPrice;
                        newitem.VisibleStatusID = stockitem.VisibleStatusID;
                        newitem.OrderTypeID = stockitem.OrderTypeID;
                        newitem.CreatedBy_UserID = stockitem.CreatedBy_UserID;
                        newitem.RegisterDate = DateTime.Now;
                        Db.StockItemTables.Add(newitem);
                        Db.SaveChanges();
                        if (stockitem.PhotoPath != null)
                        {
                            var folder = "~/Content/ItemPhoto";
                            var photoname = string.Format("{0}.jpg", newitem.StockItemID);
                            var response = HelperClass.FileUpload.UploadPhoto(stockitem.PhotoPath, folder, photoname);
                            if (response)
                            {
                                var photo = string.Format("{0}/{1}", folder, photoname);
                                newitem.ItemPhotoPath = photo;
                                Db.Entry(newitem).State = System.Data.Entity.EntityState.Modified;
                                Db.SaveChanges();
                            }
                        }
                        return RedirectToAction("StockItem", "Stock", new { id = 0 });
                    }
                    else
                    {
                        ModelState.AddModelError("StockItemTitle", "Stock Item already registered.");
                    }
                }
                else
                {
                    var checkexist = Db.StockItemTables.Where(x => x.StockItemTitle.Trim().ToUpper() == stockitem.StockItemTitle.Trim().ToUpper()
                            && x.StockItemCategoryID == stockitem.StockItemCategoryID
                            && x.OrderTypeID == stockitem.OrderTypeID
                            && x.ItemSize == stockitem.ItemSize
                            && x.StockItemID != stockitem.StockItemID).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var edititem = Db.StockItemTables.Find(stockitem.StockItemID);
                        edititem.StockItemCategoryID = stockitem.StockItemCategoryID;
                        edititem.StockItemTitle = stockitem.StockItemTitle.Trim();
                        edititem.ItemSize = stockitem.ItemSize.Trim();
                        edititem.UnitPrice = stockitem.UnitPrice;
                        edititem.VisibleStatusID = stockitem.VisibleStatusID;
                        edititem.OrderTypeID = stockitem.OrderTypeID;
                        Db.Entry(edititem).State = System.Data.Entity.EntityState.Modified;
                        Db.SaveChanges();
                        if (stockitem.PhotoPath != null)
                        {
                            var folder = "~/Content/ItemPhoto";
                            var photoname = string.Format("{0}.jpg", edititem.StockItemID);
                            var response = HelperClass.FileUpload.UploadPhoto(stockitem.PhotoPath, folder, photoname);
                            if (response)
                            {
                                var photo = string.Format("{0}/{1}", folder, photoname);
                                edititem.ItemPhotoPath = photo;
                                Db.Entry(edititem).State = System.Data.Entity.EntityState.Modified;
                                Db.SaveChanges();
                            }
                        }
                        return RedirectToAction("StockItem", "Stock", new { id = 0 });
                    }
                    else
                    {
                        ModelState.AddModelError("StockItemTitle", "Stock Item already registered.");
                    }
                }
            }
            ViewBag.StockItemCategoryID = new SelectList(Db.StockItemCategoryTables.ToList(), "StockItemCategoryID", "StockItemCategory", stockitem.StockItemCategoryID);
            ViewBag.VisibleStatusID = new SelectList(Db.VisibleStatusTables.ToList(), "VisibleStatusID", "VisibleStatus", stockitem.VisibleStatusID);
            ViewBag.OrderTypeID = new SelectList(Db.OrderTypeTables.ToList(), "OrderTypeID", "OrderTpe", stockitem.OrderTypeID);

            return View(stockitem);
        }

        public ActionResult StockItemIngredient(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            var list = new CRU_StockItemIngredientMV(id);
            return View(list);
        }
        [HttpPost]
        public ActionResult StockItemIngredient(CRU_StockItemIngredientMV cRU_StockItemIngredientMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);

            if (ModelState.IsValid)
            {
                if (cRU_StockItemIngredientMV.StockItemID > 0)
                {
                    var checkexist = Db.StockItemIngredientTables.Where(
                        s => s.StockItemIngredientTitle.Trim().ToUpper() == cRU_StockItemIngredientMV.StockItemIngredientTitle.Trim().ToUpper()
                        && s.StockItemID == cRU_StockItemIngredientMV.StockItemID).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var newingredient = new StockItemIngredientTable();
                        newingredient.StockItemID = cRU_StockItemIngredientMV.StockItemID;
                        newingredient.StockItemIngredientPhotoPath = "Content/StockIngredientPhoto/default.png";
                        newingredient.StockItemIngredientTitle = cRU_StockItemIngredientMV.StockItemIngredientTitle;
                        newingredient.CreatedBy_UserID = userid;
                        Db.StockItemIngredientTables.Add(newingredient);
                        Db.SaveChanges();
                        if (cRU_StockItemIngredientMV.PhotoPath != null)
                        {
                            var folder = "~/Content/StockIngredientPhoto";
                            var photoname = string.Format("{0}.jpg", newingredient.StockItemIngredientID);
                            var response = HelperClass.FileUpload.UploadPhoto(cRU_StockItemIngredientMV.PhotoPath, folder, photoname);
                            if (response)
                            {
                                var photo = string.Format("{0}/{1}", folder, photoname);
                                newingredient.StockItemIngredientPhotoPath = photo;
                                Db.Entry(newingredient).State = System.Data.Entity.EntityState.Modified;
                                Db.SaveChanges();
                            }
                        }
                        return RedirectToAction("StockItemIngredient", new { id = cRU_StockItemIngredientMV.StockItemID });
                    }
                    else
                    {
                        ModelState.AddModelError("StockItemIngredientTitle", "Already Exist!");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Close and Select Item First!");
                }
            }
            var list = new CRU_StockItemIngredientMV(cRU_StockItemIngredientMV.StockItemID);
            list.StockItemIngredientTitle = cRU_StockItemIngredientMV.StockItemIngredientTitle;
            list.StockItemID = cRU_StockItemIngredientMV.StockItemID;
            list.PhotoPath = cRU_StockItemIngredientMV.PhotoPath;
            return View(list);
        }
        public ActionResult DeleteIngredient(int? id)
        {
            var ingredient = Db.StockItemIngredientTables.Find(id);
            Db.Entry(ingredient).State = System.Data.Entity.EntityState.Deleted;
            Db.SaveChanges();
            return RedirectToAction("StockItemIngredient", new { id = ingredient.StockItemID });
        }

        public ActionResult StockMenu(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            var stockmenuitem = new CRU_StockMenuItemMV(id);
            ViewBag.StockMenuCategoryID = new SelectList(Db.StockMenuCategoryTables.ToList(), "StockMenuCategoryID", "StockMenuCategory", stockmenuitem.StockMenuCategoryID);
            ViewBag.StockItemID = new SelectList(Db.StockItemTables.ToList(), "StockItemID", "StockItemTitle", stockmenuitem.StockItemID);
            ViewBag.VisibleStatusID = new SelectList(Db.VisibleStatusTables.ToList(), "VisibleStatusID", "VisibleStatus", stockmenuitem.VisibleStatusID);
            return View(stockmenuitem);
        }

        [HttpPost]
        public ActionResult StockMenu(CRU_StockMenuItemMV cru_StockMenuItemMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            if (ModelState.IsValid)
            {
                if (cru_StockMenuItemMV.StockMenuItemID == 0)
                {
                    var checkexist = Db.StockMenuItemTables.Where(
                        s => s.StockMenuCategoryID == cru_StockMenuItemMV.StockMenuCategoryID
                        && s.StockItemID == cru_StockMenuItemMV.StockItemID).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var newitem = new StockMenuItemTable();
                        newitem.StockMenuCategoryID = cru_StockMenuItemMV.StockMenuCategoryID;
                        newitem.StockItemID = cru_StockMenuItemMV.StockItemID;
                        newitem.VisibleStatusID = cru_StockMenuItemMV.VisibleStatusID;
                        newitem.CreatedBy_UserID = userid;
                        Db.StockMenuItemTables.Add(newitem);
                        Db.SaveChanges();
                        return RedirectToAction("StockMenu", new { id = 0 });
                    }
                    else
                    {
                        ModelState.AddModelError("StockItemID", "Already Exist!");
                    }
                }
                else
                {
                    var checkexist = Db.StockMenuItemTables.Where(
                        s => s.StockMenuCategoryID == cru_StockMenuItemMV.StockMenuCategoryID
                        && s.StockItemID == cru_StockMenuItemMV.StockItemID
                        && s.StockMenuItemID != cru_StockMenuItemMV.StockMenuItemID).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var edititem = Db.StockMenuItemTables.Find(cru_StockMenuItemMV.StockMenuItemID);
                        edititem.StockMenuCategoryID = cru_StockMenuItemMV.StockMenuCategoryID;
                        edititem.StockItemID = cru_StockMenuItemMV.StockItemID;
                        edititem.VisibleStatusID = cru_StockMenuItemMV.VisibleStatusID;
                        edititem.CreatedBy_UserID = userid;
                        Db.Entry(edititem).State = System.Data.Entity.EntityState.Modified;
                        Db.SaveChanges();
                        return RedirectToAction("StockMenu", new { id = 0 });
                    }
                    else
                    {
                        ModelState.AddModelError("StockItemID", "Already Exist!");
                    }
                }
            }
            ViewBag.StockMenuCategoryID = new SelectList(Db.StockMenuCategoryTables.ToList(), "StockMenuCategoryID", "StockMenuCategory", cru_StockMenuItemMV.StockMenuCategoryID);
            ViewBag.StockItemID = new SelectList(Db.StockItemTables.ToList(), "StockItemID", "StockItemTitle", cru_StockMenuItemMV.StockItemID);
            ViewBag.VisibleStatusID = new SelectList(Db.VisibleStatusTables.ToList(), "VisibleStatusID", "VisibleStatus", cru_StockMenuItemMV.VisibleStatusID);
            return View(cru_StockMenuItemMV);
        }

        [HttpGet]
        public ActionResult StockDeal(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            var stockdeals = new CRU_StockDealMV(id);
            ViewBag.VisibleStatusID = new SelectList(Db.VisibleStatusTables.ToList(), "VisibleStatusID", "VisibleStatus", stockdeals.VisibleStatusID);
            return View(stockdeals);
        }

        [HttpPost]
        public ActionResult StockDeal(CRU_StockDealMV cru_StockDealMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            if (ModelState.IsValid)
            {
                if (cru_StockDealMV.StockDealID == 0)
                {
                    var checkexist = Db.StockDealTables.Where(
                        s => s.StockDealTitle == cru_StockDealMV.StockDealTitle).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var newdeal = new StockDealTable();
                        newdeal.StockDealTitle = cru_StockDealMV.StockDealTitle;
                        newdeal.DealPrice = cru_StockDealMV.DealPrice;
                        newdeal.VisibleStatusID = cru_StockDealMV.VisibleStatusID;
                        newdeal.StockDealStartDate = cru_StockDealMV.StockDealStartDate;
                        newdeal.Discount = cru_StockDealMV.Discount;
                        newdeal.StockDealEndDate = cru_StockDealMV.StockDealEndDate;
                        newdeal.StockDealRegisterDate = cru_StockDealMV.StockDealRegisterDate;
                        Db.StockDealTables.Add(newdeal);
                        Db.SaveChanges();
                        if (cru_StockDealMV.PhotoPath != null)
                        {
                            var folder = "~/Content/Deals";
                            var photoname = string.Format("{0}.jpg", newdeal.StockDealID);
                            var response = HelperClass.FileUpload.UploadPhoto(cru_StockDealMV.PhotoPath, folder, photoname);
                            if (response)
                            {
                                var photo = string.Format("{0}/{1}", folder, photoname);
                                newdeal.DealPhoto = photo;
                                Db.Entry(newdeal).State = System.Data.Entity.EntityState.Modified;
                                Db.SaveChanges();
                            }
                        }
                        return RedirectToAction("StockDeal", new { id = 0 });
                    }
                    else
                    {
                        ModelState.AddModelError("StockDealTitle", "Already Exist!");
                    }
                }
                else
                {
                    var checkexist = Db.StockDealTables.Where(
                        s => s.StockDealTitle == cru_StockDealMV.StockDealTitle
                        && s.StockDealID != cru_StockDealMV.StockDealID).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var editdeal = Db.StockDealTables.Find(cru_StockDealMV.StockDealID);
                        editdeal.StockDealTitle = cru_StockDealMV.StockDealTitle;
                        editdeal.DealPrice = cru_StockDealMV.DealPrice;
                        editdeal.VisibleStatusID = cru_StockDealMV.VisibleStatusID;
                        editdeal.StockDealStartDate = cru_StockDealMV.StockDealStartDate;
                        editdeal.Discount = cru_StockDealMV.Discount;
                        editdeal.StockDealEndDate = cru_StockDealMV.StockDealEndDate;
                        editdeal.StockDealRegisterDate = cru_StockDealMV.StockDealRegisterDate;
                        Db.Entry(editdeal).State = System.Data.Entity.EntityState.Modified;
                        Db.SaveChanges();
                        if (cru_StockDealMV.PhotoPath != null)
                        {
                            var folder = "~/Content/Deals";
                            var photoname = string.Format("{0}.jpg", editdeal.StockDealID);
                            var response = HelperClass.FileUpload.UploadPhoto(cru_StockDealMV.PhotoPath, folder, photoname);
                            if (response)
                            {
                                var photo = string.Format("{0}/{1}", folder, photoname);
                                editdeal.DealPhoto = photo;
                                Db.Entry(editdeal).State = System.Data.Entity.EntityState.Modified;
                                Db.SaveChanges();
                            }
                        }
                        return RedirectToAction("StockDeal", new { id = 0 });
                    }
                    else
                    {
                        ModelState.AddModelError("StockDealTitle", "Already Exist!");
                    }
                }
            }
            ViewBag.VisibleStatusID = new SelectList(Db.VisibleStatusTables.ToList(), "VisibleStatusID", "VisibleStatus", cru_StockDealMV.VisibleStatusID);
            return View(cru_StockDealMV);
        }

        [HttpGet]
        public ActionResult StockDealItem(int dealid, int stockdealdetailid)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            var stockdealdetails = new CRU_StockDealDetailMV(dealid, stockdealdetailid);
            ViewBag.VisibleStatusID = new SelectList(Db.VisibleStatusTables.ToList(), "VisibleStatusID", "VisibleStatus", stockdealdetails.VisibleStatusID);
            ViewBag.StockItemID = new SelectList(Db.StockItemTables.ToList(), "StockItemID", "StockItemTitle", stockdealdetails.StockItemID);
            return View(stockdealdetails);
        }

        [HttpPost]
        public ActionResult StockDealItem(CRU_StockDealDetailMV cru_StockDealDetailMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            if (ModelState.IsValid)
            {
                if (cru_StockDealDetailMV.StockDealDetailID == 0)
                {
                    var checkexist = Db.StockDealDetailTables.Where(
                        s => s.StockItemID == cru_StockDealDetailMV.StockItemID
                        && s.VisibleStatusID == 1
                        && s.StockDealID == cru_StockDealDetailMV.StockItemID).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var newitem = new StockDealDetailTable();
                        newitem.StockDealID = cru_StockDealDetailMV.StockDealID;
                        newitem.StockItemID = cru_StockDealDetailMV.StockItemID;
                        newitem.Discount = cru_StockDealDetailMV.Discount;
                        newitem.Quantity = cru_StockDealDetailMV.Quantity;
                        newitem.VisibleStatusID = cru_StockDealDetailMV.VisibleStatusID;
                        Db.StockDealDetailTables.Add(newitem);
                        Db.SaveChanges();
                        return RedirectToAction("StockDealItem", new { dealid = cru_StockDealDetailMV.StockDealID, stockdealdetailid = 0 });
                    }
                    else
                    {
                        ModelState.AddModelError("StockItemID", "Already Exist!");
                    }
                }
                else
                {
                    var checkexist = Db.StockDealDetailTables.Where(
                       s => s.StockItemID == cru_StockDealDetailMV.StockItemID
                       && s.VisibleStatusID == 1
                       && s.StockDealDetailID != cru_StockDealDetailMV.StockDealDetailID).FirstOrDefault();
                    if (checkexist == null)
                    {
                        var edititem = Db.StockDealDetailTables.Find(cru_StockDealDetailMV.StockDealDetailID);
                        edititem.StockDealID = cru_StockDealDetailMV.StockDealID;
                        edititem.StockItemID = cru_StockDealDetailMV.StockItemID;
                        edititem.Discount = cru_StockDealDetailMV.Discount;
                        edititem.Quantity = cru_StockDealDetailMV.Quantity;
                        edititem.VisibleStatusID = cru_StockDealDetailMV.VisibleStatusID;
                        Db.Entry(edititem).State = System.Data.Entity.EntityState.Modified;
                        Db.SaveChanges();
                        return RedirectToAction("StockDealItem", new { dealid = cru_StockDealDetailMV.StockDealID, stockdealdetailid = 0 });
                    }
                    else
                    {
                        ModelState.AddModelError("StockItemID", "Already Exist!");
                    }
                }
            }
            ViewBag.VisibleStatusID = new SelectList(Db.VisibleStatusTables.ToList(), "VisibleStatusID", "VisibleStatus", cru_StockDealDetailMV.VisibleStatusID);
            ViewBag.StockItemID = new SelectList(Db.StockItemTables.ToList(), "StockItemID", "StockItemTitle", cru_StockDealDetailMV.StockItemID);
            return View(cru_StockDealDetailMV);
        }
    }
}