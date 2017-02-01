using CMP.Operation.Functions.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMP.Operation.Models
{
    public class RoleViewModel
    {
        public int ID { get; set; }
        [Required]
        [MinLength(2)]
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }
}