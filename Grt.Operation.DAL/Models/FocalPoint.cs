using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.Models
{
    [Table("FocalPoint")]
    public class FocalPoint
    {
        #region Properties
        [Key]
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int? CountryID { get; set; }
        public int? StateID { get; set; }
        public int? CityID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; } = true;
        public string Remarks { get; set; }
        #endregion

        #region relationships
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("CountryID")]
        public virtual Country Country { get; set; }
        [ForeignKey("StateID")]
        public virtual State State { get; set; }
        [ForeignKey("CityID")]
        public virtual City City { get; set; }

        public virtual IEnumerable<Project> Projects { get; set; }
        #endregion
    }
}
