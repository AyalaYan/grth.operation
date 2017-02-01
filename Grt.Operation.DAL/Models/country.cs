using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.Models
{
    [Table("Country")]
    public class Country
    {
        #region Properties
        [Key]
        public int ID { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public bool IsSystem { get; set; }
        public bool IsActive { get; set; } = true;
        #endregion

        #region relationships
        //[ForeignKey("TableNameID")]
        //public virtual TableName TableName { get; set; }

        public virtual IEnumerable<State> States { get; set; }
        #endregion
    }
}
