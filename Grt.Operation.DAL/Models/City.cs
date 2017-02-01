using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.Models
{
    [Table("City")]
    public class City
    {
        #region Properties
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int StateID { get; set; }
        public bool IsSystem { get; set; }
        public bool IsActive { get; set; } = true;
        #endregion

        #region relationships
        [ForeignKey("StateID")]
        public virtual State State { get; set; }

        //public virtual IEnumerable<TableName> TableNames { get; set; }
        #endregion
    }
}
