using System.ComponentModel.DataAnnotations;

namespace EmployeeApp.Api.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        [Required]
        public string DepartmentName { get; set; }
        public string Location { get; set; }
    }
}
