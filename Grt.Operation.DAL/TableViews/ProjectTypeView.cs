using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMP.Operation.DAL.TableViews
{
    [Serializable]
    [ComplexType]
    public class ProjectTypeView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}
