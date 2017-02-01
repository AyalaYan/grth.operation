using CMP.Operation.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.Models
{
    [Table("Job")]
    public class Job
    {
        #region Properties
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        #endregion

        #region Constructors
        public Job() { }
        public Job(string Name)
        {
            this.Name = Name;
        }
        #endregion

        #region relationships

        public virtual IEnumerable<Employee> Employees { get; set; }
        #endregion
    }
}
