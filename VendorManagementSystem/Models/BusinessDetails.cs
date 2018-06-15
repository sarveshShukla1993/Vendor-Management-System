using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.Models
{
    [Table("VendorBusinessDetails")]
    public class BusinessDetails
    {
        [Key]
        public int BusinessList { get; set; }
        public int VendorId { get; set; }
        public string BusinessName { get; set; }
        public string GSTINProvisionalID { get; set; }
        public string GSTINDucument { get; set; }
        public string TANNumber { get; set; }
        public string TANDocument { get; set; }
        public string CIN { get; set; }
        public string CINDocument { get; set; }
        public string RegisteredBusineesAddress { get; set; }
        public bool DataCompleted { get; set; }
    }
}