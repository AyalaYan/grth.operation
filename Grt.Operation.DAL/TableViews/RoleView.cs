using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.TableViews
{
    [Serializable]
    [ComplexType]
    public class RoleView
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystem { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}
