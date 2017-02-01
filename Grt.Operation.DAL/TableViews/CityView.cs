using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMP.Operation.DAL.TableViews
{
    [Serializable]
    [ComplexType]
    public class CityView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public bool IsSystem { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}
