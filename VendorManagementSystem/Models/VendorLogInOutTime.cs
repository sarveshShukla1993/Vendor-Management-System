using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.Models
{
    [Table("VendorLogINOUTTime")]
    public class VendorLogInOutTime
    {
        [Key]
        public int ListId { get; set; }
        [Display(Name = "Vendor Id")]
        public int VendorId { get; set; }
        [Display(Name = "Login Time")]
        public DateTime? LogInTime { get; set; }
        [Display(Name = "Logout Time")]
        public DateTime? LogOutTime { get; set; }
        [NotMapped]
        [Display(Name = "Total Time Spent")]
        public TimeSpan? totolTime { get { return (LogOutTime - LogInTime); } set { } }
    }
}