using System;
using System.ComponentModel.DataAnnotations;

namespace AzR.AuditLog.Business.Models
{
    public class SampleViewModel
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }
        public bool Active { get; set; }
    }


}