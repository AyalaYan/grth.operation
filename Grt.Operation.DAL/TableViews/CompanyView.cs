using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMP.Operation.DAL.TableViews
{
    [Serializable]
    [ComplexType]
    public class CompanyView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}
