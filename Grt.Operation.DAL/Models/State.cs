using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.Models
{
    [Table("State")]
    public class State
    {
        #region Properties
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int CountryID { get; set; }
        public bool IsSystem { get; set; }
        public bool IsActive { get; set; } = true;
        #endregion

        #region relationships
        [ForeignKey("CountryID")]
        public virtual Country Country { get; set; }

        public virtual IEnumerable<City> Cities { get; set; }
        #endregion
    }
}
