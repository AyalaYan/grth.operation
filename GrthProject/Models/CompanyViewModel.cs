using CMP.Operation.Functions.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMP.Operation.Models
{
    public class CompanyViewModel
    {
        public int ID { get; set; }
        [Required]
        [MinLength(2)]
        [CustomRemote("IsNameCompanyAvailble", "Admin", ErrorMessage = "Name Already Exist.")]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}