using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendorManagementSystem.Models;

namespace VendorManagementSystem.ProjectViewModel
{
    public class RegisteLoginView
    {
    }

    [MetadataType(typeof(userMetaData))]
    public class VisitorView
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        [StringLength(25, ErrorMessage = "Name length must not be greater than 25")]
        public string PersonName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address")]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^[0-9]{1,2}$", ErrorMessage = "Please enter valid age.")]
        public string PersonAge { get; set; }
        [Required]
        public string Purpose { get; set; }
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use letters only please")]
        [StringLength(25, ErrorMessage = "Name length must not be greater than 25")]
        public string ContactPerson { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime MeetingDate { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public TimeSpan MeetingTime { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9, ]+$", ErrorMessage = "Use alphanumeric value only please")]
        [StringLength(60, ErrorMessage = "Name length must not be greater than 60")]
        public string AddressLine { get; set; }
        
    }

}