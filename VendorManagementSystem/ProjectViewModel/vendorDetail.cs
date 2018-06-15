using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.ProjectViewModel
{
    public class IndivisualvendorDetailModel
    {
        //general 
        [Key]
        [Display(Name = "Vendor ID")]
        public int VId { get; set; }
        [Display(Name = "Vendor Name")]
        public string VName { get; set; }
        [Display(Name = "Vendor Email")]
        public string VEmail { get; set; }
        [Display(Name = "Vendor Contact")]
        public string VContact { get; set; }
        [Display(Name = "Selling categry")]
        public string SellCategory { get; set; }
        //product
        [Display(Name = "Number of Products")]
        public string NoProducts { get; set; }
        [Display(Name = "Minimum price Range")]
        public string MiPrice { get; set; }
        [Display(Name = "Maximum Price Range")]
        public string MaPrice { get; set; }
        [Display(Name = "Are you Online seller?")]
        public string SellingOnline { get; set; }
        [Display(Name = "Business Mode")]
        public string BusinessMode { get; set; }
        [Display(Name = "Operating Location")]
        public string OperateFrom { get; set; }
        [Display(Name = "Who Handles?")]
        public string HandledBy { get; set; }
        [Display(Name = "Gender")]
        public string Gen { get; set; }
        [Display(Name = "Age")]
        public int Ag { get; set; }
        //Bank
        [Display(Name = "Account Holder Name")]
        public string AccountHolderName { get; set; }
        [Display(Name = "Account Number")]
        public string AccountNo { get; set; }
        [Display(Name = "IFSC Code")]
        public string IFSCcode { get; set; }
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Display(Name = "Pancard number")]
        public string Pancard { get; set; }
        public int panId { get; set; }
        [Display(Name = "Pancard Document")]
        public string PancardDoc { get; set; }
        [Display(Name = "Address Proof")]
        public string AddressProofId { get; set; }
        [Display(Name = "Address Proof Document")]
        public string AddressProofDoc { get; set; }
        [Display(Name = "Cancelled Cheque")]
        public string CancelCheque { get; set; }
        //Business
        [Display(Name = "Company/Retail Name")]
        public string CompanyName { get; set; }
        public int BusId { get; set; }
        [Display(Name = "GST Document Number")]
        public string GSTProvisionalID { get; set; }
        [Display(Name = "GST Document")]
        public string GSTDucument { get; set; }
        [Display(Name = "TAN Number")]
        public string TANNo { get; set; }
        [Display(Name = "TAN Document")]
        public string TANDoc { get; set; }
        [Display(Name = "CIN Number")]
        public string CINNo { get; set; }
        [Display(Name = "CIN Document")]
        public string CINDoc { get; set; }
        [Display(Name = "Registered Company/Retail address")]
        public string RegisteredBusineesAddr { get; set; }
    }
}