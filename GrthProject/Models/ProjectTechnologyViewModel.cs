using CMP.Operation.Functions.Filters;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMP.Operation.Models
{
    public class ProjectTechnologyViewModel
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        [CustomRemote("IsNameProjectTechnologyAvailble", "Admin", ErrorMessage = "Company Already Exist.")]
        public int TechnologyID { get; set; }
        public string TechnologyName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowDelete { get; set; }

    }
}