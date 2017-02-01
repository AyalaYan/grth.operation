using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.Models
{
    [Table("Role")]
    public class Role
    {
        #region Properties
        [Key]
        public int ID { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        #endregion

        #region relationships
        public virtual IEnumerable<User> Users { get; set; }
        #endregion
    }
}
