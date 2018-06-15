using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VendorManagementSystem.Models
{
    [Table("ContactPersonName")]
    public class contactpersonname
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}