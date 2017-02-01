
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMP.Operation.DAL.TableViews
{
    [Serializable]
    [ComplexType]
    public class ProjectTechnologyView
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int TechnologyID { get; set; }
        public string TechnologyName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}
