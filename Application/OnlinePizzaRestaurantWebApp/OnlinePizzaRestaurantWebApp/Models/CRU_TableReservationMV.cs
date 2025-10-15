using DBlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlinePizzaRestaurantWebApp.Models
{
    public class CRU_TableReservationMV
    {
        OnlinePizzaRestaurantDbEntities db = new OnlinePizzaRestaurantDbEntities();
        public CRU_TableReservationMV() { }
        public CRU_TableReservationMV(int usertypeid, int userid, int id = 0)
        {
            GetReservationList(usertypeid, userid);
            if (userid > 0)
            {
                var user = db.UserTables.Find(userid);
                FullName = user.FirstName + " " + user.LastName;
                EmailAddress = user.EmailAddress;
                MobileNo = user.ContactNo;
            }
            ReservationDate = null;
            ReservationTime = null;
            var edit = db.TableReservationTables.Find(id);
            if (edit != null)
            {
                BookingTableID = edit.TableReservationID;
                FullName = edit.FullName;
                EmailAddress = edit.EmailAddress;
                MobileNo = edit.MobileNo;
                ReservationDate = edit.ReservationDateTime.Date;
                ReservationTime = edit.ReservationDateTime.ToString("hh:mm:ss tt");
                NoOfPersons = edit.NoOfPeople;
                BookingStatusID = edit.ReservationStatusID;
                Description = edit.Description;
            }
            else
            {
                BookingTableID = 0;
                FullName = string.Empty;
                EmailAddress = string.Empty;
                MobileNo = string.Empty;
                ReservationDate = null;
                ReservationTime = string.Empty;
                NoOfPersons = null;
                BookingStatusID = 0;
                Description = string.Empty;
            }
        }
        public int BookingTableID { get; set; }
        [DataType(DataType.Text)]
        public string FullName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        public string MobileNo { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime BookingDate { get; set; }
        public System.DateTime? ReservationDate { get; set; }
        public string ReservationTime { get; set; }
        public int? NoOfPersons { get; set; }
        public int BookingStatusID { get; set; }
        public string Description { get; set; }

        public List<TableReservationMV> ReservationList { get; set; }
        public void GetReservationList(int usertypeid, int userid)
        {
            ReservationList = new List<TableReservationMV>();
            if (usertypeid == 3) // Customer
            {
                var list = db.TableReservationTables.Where(u => u.ReservationUserID == userid).ToList();
                if (list.Count > 0)
                {
                    list.OrderByDescending(o => o.TableReservationID).ToList();
                }
                foreach (var item in list)
                {
                    var bookingusername = item.UserTable.UserName;

                    var processbyuser = item.ProcessBy_UserID > 0 ?
                        db.UserTables.Find(item.ProcessBy_UserID).UserName :
                        string.Empty;

                    var bookingstatus = item.ReservationStatusTable.ReservationStatus;
                    ReservationList.Add(new TableReservationMV
                    {
                        BookingTableID = item.TableReservationID,
                        BookingUserName = bookingusername,
                        FullName = item.FullName,
                        EmailAddress = item.EmailAddress,
                        MobileNo = item.MobileNo,
                        BookingDate = item.ReservationRequestDate,
                        ReservationDateTime = item.ReservationDateTime,
                        NoOfPersons = item.NoOfPeople,
                        ProcessBy_User = processbyuser,
                        BookingStatus = bookingstatus,
                        Description = item.Description
                    });
                }
            }
            else if (usertypeid == 1 || usertypeid == 2)
            {
                var list = db.TableReservationTables.ToList();
                if (list.Count > 0)
                {
                    list.OrderByDescending(o => o.TableReservationID).ToList();
                }
                foreach (var item in list)
                {
                    var bookingusername = item.UserTable.UserName;

                    var processbyuser = item.ProcessBy_UserID > 0 ?
                        db.UserTables.Find(item.ProcessBy_UserID).UserName :
                        string.Empty;

                    var bookingstatus = item.ReservationStatusTable.ReservationStatus;
                    ReservationList.Add(new TableReservationMV
                    {
                        BookingTableID = item.TableReservationID,
                        BookingUserName = bookingusername,
                        FullName = item.FullName,
                        EmailAddress = item.EmailAddress,
                        MobileNo = item.MobileNo,
                        BookingDate = item.ReservationRequestDate,
                        ReservationDateTime = item.ReservationDateTime,
                        NoOfPersons = item.NoOfPeople,
                        ProcessBy_User = processbyuser,
                        BookingStatus = bookingstatus,
                        Description = item.Description
                    });
                }
            }
        }
    }
}