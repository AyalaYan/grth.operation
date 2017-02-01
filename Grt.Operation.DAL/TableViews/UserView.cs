using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMP.Operation.DAL.TableViews
{
    [Serializable]
    [ComplexType]
    public class UserView
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}
