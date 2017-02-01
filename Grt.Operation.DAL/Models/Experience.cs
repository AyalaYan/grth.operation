using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.Models
{
    [Table("Experience")]
    public class Experience
    {
        #region Properties
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        [Key, Column(Order = 1)]
        public int ID { get; set; }
        [Index("IX_Experience_Employee_Company", IsUnique = true, Order = 1)]
        public int EmployeeID { get; set; }
        // [Key, Column(Order = 2)]
        [Index("IX_Experience_Employee_Company", IsUnique = true, Order = 2)]
        public int CompanyID { get; set; }
        [Index("IX_Experience_Employee_Company", IsUnique = true, Order = 3)]
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IsActive { get; set; } = true;
        #endregion

        #region relationships
        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("CompanyID")]
        public virtual Company Company { get; set; }

        public virtual ICollection<ExperienceTechnology> ExperienceTechnologys { get; set; }
        #endregion
    }
}
