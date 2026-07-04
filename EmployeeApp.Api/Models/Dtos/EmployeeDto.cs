using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeApp.Api.Models.Dtos
{
    public class EmployeeDto
    {
       
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"[A-Z][A-Za-z\s]+", ErrorMessage = "Name should contain only Alphabets")]

        public string Name { get; set; }
        [RegularExpression("(Male|Female)", ErrorMessage = "Gender should be either Male or Female")]
        public string Gender { get; set; }
        [Range(18, 60, ErrorMessage = "Age should  be between 18 and 60")]
        public int? Age { get; set; }
       
        public decimal Salary { get; set; }
        public int DeptId { get; set; }
        
       
    }
}
