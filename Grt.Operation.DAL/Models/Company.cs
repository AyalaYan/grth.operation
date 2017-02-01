using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.Models
{
    [Table("Company")]
    public class Company
    {
        #region Properties
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int? CustomerID { get; set; }
        public bool IsActive { get; set; } = true;
        #endregion

        #region relationships
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        public virtual IEnumerable<Experience> Experiences { get; set; }
        #endregion
    }
}
