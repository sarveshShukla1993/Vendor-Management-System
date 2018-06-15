using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.Models
{
    [Table("adminlogin")]
    public class AdminLogin
    {
        [Key]
        public int id { get; set; }
        public string EmailAddress { get; set; }
        public string Passkey { get; set; }
    }
}