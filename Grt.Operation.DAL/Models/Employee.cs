using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.Models
{
    [Table("Employee")]
    public class Employee
    {
        #region Properties
        [Key]
        public int ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FirstFamilyName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int JobID { get; set; }
        public int? CountryID { get; set; }
        public int? StateID { get; set; }
        public int? CityID { get; set; }
        public bool IsActive { get; set; } = true;
        public string Remarks { get; set; }
        #endregion

        #region relationships
        [ForeignKey("JobID")]
        public virtual Job Job { get; set; }

        [ForeignKey("CountryID")]
        public virtual Country Country { get; set; }
        [ForeignKey("StateID")]
        public virtual State State { get; set; }
        [ForeignKey("CityID")]
        public virtual City City { get; set; }

        public virtual IEnumerable<Project> Projects { get; set; }
        public virtual IEnumerable<Experience> Experiences { get; set; }
        #endregion
    }
}
