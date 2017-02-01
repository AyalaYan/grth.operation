using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.Models
{
    [Table("ProjectTechnology")]
    public class ProjectTechnology
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column(Order = 0)]
        public int ID { get; set; }
        [Key, Column(Order = 1)]
        public int ProjectID { get; set; }
        [Key, Column(Order = 2)]
        public int TechnologyID { get; set; }
        public bool IsActive { get; set; } 
        #endregion

        #region relationships
        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }
        [ForeignKey("TechnologyID")]
        public virtual Technology Technology { get; set; }

        //public virtual IEnumerable<TableName> TableNames { get; set; }
        #endregion
    }
}
