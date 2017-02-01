using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMP.Operation.DAL.TableViews
{
    [Serializable]
    [ComplexType]
    public class Options
    {
        public int Value { get; set; }
        public string DisplayText { get; set; }
       
    }
}
