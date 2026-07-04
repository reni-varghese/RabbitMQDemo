using EmployeeApp.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApp.Api.Data
{
    public class AppDbContext :IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<FullTimeEmployee> FullTimeEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>()
                .HasData(

                    new Employee {Id=1, Name="Mary",Gender="Female",Age=23,Salary=78000,DeptId=101},
                    new Employee {Id=2, Name="Arun",Gender="Male",Age=26,Salary=45000,DeptId=102},
                    new Employee {Id=3, Name="Shivani",Gender="Female",Age=29,Salary=87000,DeptId=103},
                    new Employee {Id=4, Name="Manu",Gender="Male",Age=33,Salary=67000,DeptId=101}

                );

            modelBuilder.Entity<Department>()
                .HasData(

                    new Department { DepartmentId=101,DepartmentName="IT",Location="Bangalore"},
                    new Department { DepartmentId=102,DepartmentName="Sales",Location="Chennai"},
                    new Department { DepartmentId=103,DepartmentName="HR",Location="Pune"},
                    new Department { DepartmentId=104,DepartmentName="Payroll",Location="Trivandrum"}

                );


            modelBuilder.Entity<FullTimeEmployee>(entity =>
            {
                entity.HasKey(e => e.EmpId);
                entity.Property(e => e.Name)
                .IsRequired(true)
                .HasAnnotation("RegularExpression", @"[A-Z][A-Za-z\s]+");

                entity.Property(e => e.Gender)
                .HasAnnotation("RegularExpression", "(Male|Female)");

                entity.Property(e => e.Age)
                .HasAnnotation("Range_Min", 18)
                .HasAnnotation("Range_Max", 60)
                .HasAnnotation("ErrorMessage", "Age should be between 18 and 60");

                entity.Property(e => e.Salary)
                .HasPrecision(18, 2);

                entity.HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DeptId);



            });


        }
    }
}
