using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.ProjectViewModel
{
    public class BusinessDetailsView
    {
        [Key]
        public int BusinessList { get; set; }
        public int VendorId { get; set; }
        //[ForeignKey("VendorId")]
        //public virtual VendorViewDetails VendorId1 { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string BusinessName { get; set; }
        [Required]
        [Display(Name = "GSTIN ID")]
        public string GSTINProvisionalID { get; set; }
        [Required]
        [Display(Name = "GST File")]
        public string GSTINDocument { get; set; }
        [Required]
        [Display(Name = "TAN Number")]
        public string TANNumber { get; set; }
        [Required]
        [Display(Name = "TAN File")]
        public string TANDocument { get; set; }
        [Required]
        [Display(Name = "CIN Number")]
        public string CIN { get; set; }
        [Required]
        [Display(Name = "CIN File")]
        public string CINDocument { get; set; }
        [Required]
        [Display(Name = "Address of Business")]
        public string RegisteredBusineesAddress { get; set; }
    }
}