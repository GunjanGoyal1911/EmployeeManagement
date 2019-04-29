using EmployeeManagement.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [ValidationHelper(Allowed = new string[] { "Asp.net", "Java", "Oracle" }, ErrorMessage = "Your skills are invalid")]
        public string Skills { get; set; }
        [Required]
        [ValidationHelper(Allowed = new string[] { "Male", "Female"}, ErrorMessage = "Write either Male or Female ")]
        public string Gender { get; set; }
        [Range(18, 55)]
        public int Age { get; set; }
        [Required]
        public decimal Salary { get; set; }
               
        public DateTime JoiningDate { get; set; }
    }
}
