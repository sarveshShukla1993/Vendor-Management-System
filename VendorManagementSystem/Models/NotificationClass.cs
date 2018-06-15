using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.Models
{
    public class NotificationClass
    {
        public void RegisterNotification(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["VMSConnection"].ConnectionString;
            string sqlCommand = @"SELECT [AccountHoderName],[AccountNumber],[IFSC],[BankName],[Country],[StateName],[PanCard],[AddressProof],[CancelledCheque],[Id],[VendorId],[AddressProofDocument],[PancardDocument] from [dbo].[VendorBankDetails] where [AddedOn] > @AddedOn";
            using (SqlConnection sqlCon = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, sqlCon);
                cmd.Parameters.AddWithValue("@AddedOn", currentTime);
                if(sqlCon.State != System.Data.ConnectionState.Open)
                {
                    sqlCon.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if(e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");
                RegisterNotification(DateTime.Now);
            }
        }

        public List<VendorBankDetails> GetContacts(DateTime afterDate)
        {
            using (DBConnection db = new DBConnection())
            {
                return db.bankdetails.Where(a => a.AddedOn > afterDate).OrderByDescending(a => a.AddedOn).ToList();

            }
        }

        //Code for product upload notification to superAdmin

        public void RegisterProductNotification(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["VMSConnection"].ConnectionString;
            string sqlCommand = @"SELECT [ProductId],[ProductName],[ProductPrice],[ProductQuantity],[ProductDescription],[ProductImage],[VendorId],[SellingCategory],[IsDeleted],[TimeOfDeletion],[OrderApproved],[OrderRecieved],[OrderDelivered] from [dbo].[ListOfProducts] where [AddedOn] > @AddedOn";
            using (SqlConnection sqlCon = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, sqlCon);
                cmd.Parameters.AddWithValue("@AddedOn", currentTime);
                if (sqlCon.State != System.Data.ConnectionState.Open)
                {
                    sqlCon.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChangeA;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                }
            }
        }

        void sqlDep_OnChangeA(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChangeA;

                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");
                RegisterProductNotification(DateTime.Now);
            }
        }

        public List<UploadProduct> GetProduct(DateTime afterDate)
        {
            using (DBConnection db = new DBConnection())
            {
                return db.uploadproduct.Where(a => a.AddedOn > afterDate).OrderByDescending(a => a.AddedOn).ToList();

            }
        }

    }
}