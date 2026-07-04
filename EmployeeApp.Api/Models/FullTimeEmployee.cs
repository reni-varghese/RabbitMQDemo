namespace EmployeeApp.Api.Models
{
    public class FullTimeEmployee
    {
        public int EmpId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public decimal Salary { get; set; }

        public int DeptId { get; set; }

        public Department? Department { get; set; }
    }
}
