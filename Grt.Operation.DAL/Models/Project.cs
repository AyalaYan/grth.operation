using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.Models
{
    [Table("Project")]
    public class Project
    {
        #region Properties
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int CustomerID { get; set; }
        public int? DepartmentID { get; set; }
        public int ProjectTypeID { get; set; }
        public int? FocalPointID { get; set; }
        public int? CompanyFocalPointID { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Risk { get; set; }

        #endregion

        #region relationships
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("DepartmentID")]
        public virtual Department Department { get; set; }
        [ForeignKey("ProjectTypeID")]
        public virtual ProjectType ProjectType { get; set; }
        [ForeignKey("FocalPointID")]
        public virtual FocalPoint FocalPoint { get; set; }
        [ForeignKey("CompanyFocalPointID")]
        public virtual Employee CompanyFocalPoint { get; set; }

        public virtual IEnumerable<ProjectTechnology> ProjectTechnologys { get; set; }
        #endregion
    }
}
