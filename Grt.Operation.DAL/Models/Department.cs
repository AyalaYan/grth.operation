using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.Models
{
    [Table("Department")]
    public class Department
    {
        #region Properties
        [Key]
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        #endregion

        #region relationships
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        public virtual IEnumerable<Project> Projects { get; set; }
        #endregion
    }
}
