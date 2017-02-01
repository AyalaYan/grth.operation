using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMP.Operation.DAL.TableViews
{
    [Serializable]
    [ComplexType]
    public class ExperienceTechnologyView
    {
        public int ID { get; set; }
        public int ExperienceID { get; set; }
        public int TechnologyID { get; set; }
        public string TechnologyName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}
