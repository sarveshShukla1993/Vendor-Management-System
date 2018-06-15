using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.ProjectViewModel
{
    public class BankDetailsView
    {
        [Key]
        public int Id { get; set; }
        public int VendorId { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter letters only.")]
        public string AccountHoderName { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter valid account number.")]
        public string AccountNumber { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter valid account number")]
        [Compare("AccountNumber", ErrorMessage = "The new account number do not match.")]
        public string ReTypeAccountNumber { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Please enter Alphanumeric value only.")]
        public string IFSC { get; set; }
        [Required]
        public string BankName { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string StateName { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Please enter Alphanumeric value only.")]
        public string PanCard { get; set; }
        [Required]
        public string PancardDocument { get; set; }
        [Required]
        public string AddressProof { get; set; }
        [Required]
        public string AddressProofDocument { get; set; }
        [Required]
        public string CancelledCheque { get; set; }

    }

    public enum Bank
    {
        HDFC,
        ICICI,
        SBI,
        UNION,
        PNB,
        AXIS
    }
    public enum AddressProof
    {
        AadharCard,
        LightBill,
        VoterId,
        rationCard
    }

}