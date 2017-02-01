using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMP.Operation.DAL.TableViews
{
    [Serializable]
    [ComplexType]
    public class ProjectView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int? DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int ProjectTypeID { get; set; }
        public string ProjectTypeName { get; set; }
        public int? FocalPointID { get; set; }
        public string FocalPointName { get; set; }
        public int? CompanyFocalPointID { get; set; }
        public string CompanyFocalPointName { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public int? Risk { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}
