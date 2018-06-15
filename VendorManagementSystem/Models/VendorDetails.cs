using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.Models
{
    [Table("VendorDetails")]
    public class VendorDetails
    {
        [Key]
        public int tableId { get; set; }
        public int VendorId { get; set; }
        public string NumberofProducts { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string AlreadySellingOnline { get; set; }
        public string NatureOfBusiness { get; set; }
        public string OperatingFrom { get; set; }
        public string HandleBy { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
    }
}