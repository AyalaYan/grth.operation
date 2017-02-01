using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.Models
{
    [Table("User")]
    public class User
    {
        #region Properties
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public int RoleID { get; set; }
        #endregion

        #region relationships
        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
        #endregion
    }
}
