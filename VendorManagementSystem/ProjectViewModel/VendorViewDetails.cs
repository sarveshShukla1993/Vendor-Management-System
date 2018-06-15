using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.ProjectViewModel
{
    public class VendorViewDetails
    {
        [Key]
        public int tableId { get; set; }
        public int VendorId { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter numeric value only.")]
        public string NumberofProducts { get; set; }
        [Required(ErrorMessage = "Min price is required")]
        [Display(Name = "Price Range")]
        [RegularExpression("^[0-9]{2,8}$", ErrorMessage = "Please enter atleast 2 digit value.")]
        public string MinPrice { get; set; }
        [Required(ErrorMessage = "Max price is required")]
        [RegularExpression("^[0-9]{2,8}$", ErrorMessage = "Please enter atleast 2 digit value.")]
        public string MaxPrice { get; set; }
        [Required]
        public string AlreadySellingOnline { get; set; }
        [Required]
        public string NatureOfBusiness { get; set; }
        [Required]
        public string OperatingFrom { get; set; }
        [Required]
        [Display(Name = "Handle By")]
        public string HandleBy { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [Display(Name = "Owner Age")]
        [Range(18,99,ErrorMessage = "Age nust be between 18 to 80")]
        [RegularExpression("^[0-9]{2}$", ErrorMessage = "Please enter age between 18 to 99 valid age")]
        public int Age { get; set; }
    }

    public enum handlyby
    {
        Self,
        Business_Partner,
        Employee
    }

    public enum OptFrom
    {
        Home,
        Retail,
        WareHouse
    }

    public enum NOBuss
    {
        Retailer,
        Online_Retailer,
        Manufacturer,
        Importer
    }
   
}