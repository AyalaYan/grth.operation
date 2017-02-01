using CMP.Operation.Functions.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMP.Operation.Models
{
    public class ProjectViewModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MinLength(2)]
        [CustomRemote("IsNameProjectAvailble", "Admin", ErrorMessage = "Name Already Exist.")]
        public string Name { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int? DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int? TechnologyID { get; set; }
        public string TechnologyName { get; set; }
        public int ProjectTypeID { get; set; }
        public string ProjectTypeName { get; set; }
        public int? FocalPointID { get; set; }
        public string FocalPointName { get; set; }
        public int? CompanyFocalPointID { get; set; }
        public string CompanyFocalPointName { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Risk { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}