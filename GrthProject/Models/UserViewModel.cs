using CMP.Operation.Functions.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMP.Operation.Models
{
    public class UserViewModel
    {
        [Key]
        public int ID { get; set; }
        //[Required]
        //[MinLength(2)]
        //public string UserName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}