using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VendorManagementSystem.Models;

namespace VendorManagementSystem.ProjectViewModel
{
    public class AdminVendorDetails
    {
        [Key]
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorEmail { get; set; }
        public string VendorContact { get; set; }
        public string SellingCategory { get; set; }
        public string BusinessName { get; set; }
    }
}