using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMP.Operation.DAL.TableViews
{
    [Serializable]
    [ComplexType]
    public class ExperienceView
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowDelete { get; set; }
        public List<int> Technologies { get; set; }
    }
}
