using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.Models
{
    [Table("ListOfProducts")]
    public class UploadProduct
    {
        [Key]
        [Display(Name = "Product Id")]
        public int ProductId { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Product Price")]
        public string ProductPrice { get; set; }
        [Display(Name = "Product Quantity")]
        public int ProductQuantity { get; set; }
        [Display(Name = "Product Description")]
        public string ProductDescription { get; set; }
        [Display(Name = "Product Image")]
        public string ProductImage { get; set; }
        public int VendorId { get; set; }
        [Display(Name = "Selling Category")]
        public string SellingCategory { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? TimeOfDeletion { get; set; }
        [Display(Name = "Order Approved")]
        public bool OrderApproved { get; set; }
        [Display(Name = "Order Recieved")]
        public bool OrderRecieved { get; set; }
        [Display(Name = "Order Delivered")]
        public bool OrderDelivered { get; set; }
        public DateTime AddedOn { get; set; }
    }
}