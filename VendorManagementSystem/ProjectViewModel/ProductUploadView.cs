using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.ProjectViewModel
{
    public class ProductUploadView
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        [Required]
        [RegularExpression("^[0-9]{3,9}$", ErrorMessage = "Please enter valid price")]
        public string ProductPrice { get; set; }
        [Required]
        [RegularExpression("^[0-9]{1,9}$", ErrorMessage = "Please enter Numeric value")]
        public int ProductQuantity { get; set; }
        public string ProductDescription { get; set; }
        [Required]
        public string ProductImage { get; set; }
        public string SellingCategory { get; set; }
        public DateTime AddedOn { get; set; }
    }

    public enum Sellingcategory
    {
        Men,
        Women,
        Kids,
        Pets
    }
}