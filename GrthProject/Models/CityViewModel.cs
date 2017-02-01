using CMP.Operation.Functions.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMP.Operation.Models
{
    public class CityViewModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [CustomRemote("IsNameCityAvailble", "Admin", ErrorMessage = "Name Already Exist.")]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public bool IsSystem { get; set; }
        public bool IsAllowDelete { get; set; }
    }
}