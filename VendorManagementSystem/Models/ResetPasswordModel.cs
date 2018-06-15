using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.Models
{
    public class ResetPasswordModel
    {
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "The Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }
        public string ResetCode { get; set; }
    }
}