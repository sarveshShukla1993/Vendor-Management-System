using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.Models
{
    public class ProductListProductUploadMethod
    {
        public VendorManagementSystem.ProjectViewModel.ProductUploadView upload { get; set; }
        public IPagedList<VendorManagementSystem.Models.UploadProduct> uploadproduct { get; set; }
    }
}