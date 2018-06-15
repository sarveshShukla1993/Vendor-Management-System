using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.Models
{
    [Table("SellingCategoryRecord")]
    public class SellingCategoryModel
    {
        [Key]
        public int Id { get; set; }
        public string hobbyname { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUsed { get; set; }
    }
}