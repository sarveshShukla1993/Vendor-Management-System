using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VendorManagementSystem.Models
{
    public class userMetaData
    {
        [Remote("IsUserNameAlreadyExists", "RegisteLogin", ErrorMessage = "Visitor already exists.")]
        public string Email { get; set; }
    }

    public class VendorMetaData
    {
        [Remote("IsVendorAlreadyExists", "RegisteLogin", ErrorMessage = "Vendor already exists.")]
        public string VendorEmail { get; set; }
    }
}