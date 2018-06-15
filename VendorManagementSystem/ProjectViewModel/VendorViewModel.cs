using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VendorManagementSystem.Models;

namespace VendorManagementSystem.ProjectViewModel
{
    [MetadataType(typeof(VendorMetaData))]
    public class VendorViewModel
    {
        [Key]
        [Display(Name = "Vendor ID")]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        [StringLength(25, ErrorMessage = "Name length must not be greater than 25")]
        public string VendorName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Please enter valid email Address")]
        public string VendorEmail { get; set; }
        [Required]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Please enter valid contact number")]
        public string VendorContact { get; set; }
        [Required]
        public string VendorPassword { get; set; }
        [Required]
        public string SellingCategory { get; set; }
    }   

    public class vendorViewLogin
    {
        [Key]
        [Display(Name = "Email Address")]
        public string VendorEmail { get; set; }
        [Display(Name = "Password")]
        public string VendorPassword { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }
}