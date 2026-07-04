using AutoMapper;
using EmployeeApp.Api.Models;
using EmployeeApp.Api.Models.Dtos;
using EmployeeApp.Api.Repositories;

namespace EmployeeApp.Api.Services
{
    public class EmployeeService(IEmployeeRepository repository,IMapper mapper) : IEmployeeService
    {

        

        public async Task<EmployeeDto> AddAsync(EmployeeDto entity)
        {
            var employee=mapper.Map<Employee>(entity);

           var savedEntity= await repository.CreateAsync(employee);
           return mapper.Map<EmployeeDto>(savedEntity);
        }

        public async Task<EmployeeDto> DeleteAsync(int id)
        {
            var deleted=await repository.DeleteAsync(id);
            return mapper.Map<EmployeeDto>(deleted);
        }

        public async Task<List<EmployeeDto>> GetAllAsync()
        {
            return mapper.Map<List<EmployeeDto>>(await repository.GetAllAsync());
        }

        public async Task<EmployeeDto> GetByIdAsync(int id)
        {
            return mapper.Map<EmployeeDto>(await repository.GetByIdAsync(id));
        }

        public async Task<EmployeeDto> UpdateAsync(int id, EmployeeDto entity)
        {
            var emp = mapper.Map<Employee>(entity);
            //emp.Id = id;
            //Employee emp = new Employee
            //{
            //    Id=id,
            //    Name = entity.Name,
            //    Gender = entity.Gender,
            //    Age = entity.Age,
            //    Salary = entity.Salary,
            //    DeptId=entity.DeptId

            //};
            var updated=await repository.UpdateAsync(id, emp);
            return mapper.Map<EmployeeDto>(updated);
        }
    }
}
