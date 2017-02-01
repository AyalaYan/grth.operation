using CMP.Operation.Functions.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMP.Operation.Models
{
    public class StateViewModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [CustomRemote("IsNameStateAvailble", "Admin", ErrorMessage = "Name Already Exist.")]
        public string Name { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public bool IsSystem { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}