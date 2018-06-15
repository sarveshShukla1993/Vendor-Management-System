using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.Models
{
    [Table("vendorRegostration")]
    public class VendorModel
    {
        [Key]
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorEmail { get; set; }
        public string VendorContact { get; set; }
        public string VendorPassword { get; set; }
        public string SellingCategory { get; set; }
        public string ResetPasswordCode { get; set; }
        public bool DataCompleted { get; set; }
    }
}