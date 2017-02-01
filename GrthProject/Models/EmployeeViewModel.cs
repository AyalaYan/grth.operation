
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMP.Operation.Models
{
    [Serializable]
    [ComplexType]
    public class EmployeeViewModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string FirstFamilyName { get; set; }

        
        [MinLength(4)]
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public int JobID { get; set; }
        public string JobName { get; set; }
        public int? CountryID { get; set; }
        public int? StateID { get; set; }
        public int? CityID { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public bool IsAllowDelete { get; set; }
        public DateTime StartDate { get; set; }

    }
}