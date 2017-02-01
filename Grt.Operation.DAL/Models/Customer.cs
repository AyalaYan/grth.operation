using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.Models
{
    [Table("Customer")]
    public class Customer
    {
        #region Properties
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        #endregion


        #region relationships
        //[ForeignKey("TableNameID")]
        //public virtual TableName TableName { get; set; }

        public virtual IEnumerable<Department> Departments { get; set; }
        public virtual IEnumerable<Project> Projects { get; set; }
        public virtual IEnumerable<FocalPoint> FocalPoints { get; set; }
        public virtual IEnumerable<Customer> Customers { get; set; }
        #endregion
    }
}
