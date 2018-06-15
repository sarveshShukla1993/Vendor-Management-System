using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.Models
{
    [Table("VendorBankDetails")]
    public class VendorBankDetails
    {
        [Key]
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string AccountHoderName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSC { get; set; }
        public string BankName { get; set; }
        public string Country { get; set; }
        public string StateName { get; set; }
        public string PanCard { get; set; }
        public string PancardDocument { get; set; }
        public string AddressProof { get; set; }
        public string AddressProofDocument { get; set; }
        public string CancelledCheque { get; set; }
        public DateTime AddedOn { get; set; }
    }

}