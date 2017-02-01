using CMP.Operation.Functions.Filters;
using System.ComponentModel.DataAnnotations;

namespace CMP.Operation.Models
{
    public class TechnologyViewModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MinLength(2)]
        [CustomRemote("IsNameTechnologyAvailble", "Admin", ErrorMessage = "Name Already Exist.")]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}