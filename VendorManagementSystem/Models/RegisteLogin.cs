using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VendorManagementSystem.Models
{

    [Table("Visitors")] 
    public class Visitor
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Age { get; set; }
        public string Purpose { get; set; }
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        [Display(Name = "Meeting Date")]
        [DataType(DataType.Date)]
        public DateTime MeetingDate { get; set; }
        [Display(Name = "Meeting Time")]
        [DataType(DataType.Time)]
        public TimeSpan MeetingTime { get; set; }
        public string AddressLine { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? TimeOfDeletion { get; set; }
        public double Count { get; set; }
    }

    //public class ExternalLoginListViewModel
    //{
    //    public string ReturnUrl { get; set; }
    //}

}