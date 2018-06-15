using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.Models
{
    public class DBConnection : DbContext 
    {
        public DBConnection() : base("VMSConnection")
        {
             Database.SetInitializer<DBConnection>(new CreateDatabaseIfNotExists<DBConnection>());
        }
        public DbSet<Visitor> visitor { get; set; }
        public DbSet<VendorModel> vendor { get; set; }
        public DbSet<VendorDetails> vendordetails { get; set; }
        public DbSet<BusinessDetails> businessdetails { get; set; }
        public DbSet<VendorBankDetails> bankdetails { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<State> states { get; set; }
        public DbSet<UploadProduct> uploadproduct { get; set; }
        public DbSet<AdminLogin> adminlog { get; set; }
        public DbSet<VisitPurpose> purpose { get; set; }
        public DbSet<SellingCategoryModel> sellcategory { get; set; }
        public DbSet<VendorLogInOutTime> loginouttime { get; set; }
        public DbSet<contactpersonname> personcontactname { get; set; }
        public DbSet<UserRoles> userrole { get; set; }
        public System.Data.Entity.DbSet<VendorManagementSystem.ProjectViewModel.VisitorView> VisitorViews { get; set; }

        public System.Data.Entity.DbSet<VendorManagementSystem.ProjectViewModel.VendorViewModel> VendorViewModels { get; set; }

        public System.Data.Entity.DbSet<VendorManagementSystem.ProjectViewModel.vendorViewLogin> vendorViewLogins { get; set; }

        public System.Data.Entity.DbSet<VendorManagementSystem.ProjectViewModel.VendorViewDetails> VendorViewDetails { get; set; }

        public System.Data.Entity.DbSet<VendorManagementSystem.ProjectViewModel.BusinessDetailsView> BusinessDetailsViews { get; set; }

        public System.Data.Entity.DbSet<VendorManagementSystem.ProjectViewModel.VendorAccessDetails> VendorAccessDetails { get; set; }

        public System.Data.Entity.DbSet<VendorManagementSystem.ProjectViewModel.ProductUploadView> ProductUploadViews { get; set; }

        public System.Data.Entity.DbSet<VendorManagementSystem.ProjectViewModel.AdminVendorDetails> AdminVendorDetails { get; set; }

        public System.Data.Entity.DbSet<VendorManagementSystem.ProjectViewModel.IndivisualvendorDetailModel> IndivisualvendorDetailModels { get; set; }
    }
}
