using CMP.Operation.Functions.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMP.Operation.Models
{
    public class ProjectTypeViewModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MinLength(2)]
        [CustomRemote("IsNameProjectTypeAvailble", "Admin", ErrorMessage = "Name Already Exist.")]
       
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}