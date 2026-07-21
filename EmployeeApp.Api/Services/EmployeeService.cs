using AutoMapper;
using EmployeeApp.Api.Messaging;
using EmployeeApp.Api.Models;
using EmployeeApp.Api.Models.Dtos;
using EmployeeApp.Api.Repositories;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace EmployeeApp.Api.Services
{
    public class EmployeeService(IEmployeeRepository repository,IMapper mapper,
        IPublishEndpoint publishEndPoint
        ) : IEmployeeService
    {

        //private const string EmployeeListCacheKey = "employees:all";
        //private static readonly DistributedCacheEntryOptions cacheOptions = new()
        //{
        //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        //};
        public async Task<EmployeeDto> AddAsync(EmployeeDto entity)
        {
            var employee=mapper.Map<Employee>(entity);

           var savedEntity= await repository.CreateAsync(employee);
            var result=mapper.Map<EmployeeDto>(savedEntity);

            await publishEndPoint.Publish(new Events.EmployeeEvent
            {
                EventType = "Created",
                EmployeeId = result.Id,
                EmployeeName = result.Name,
                OccurredAt = DateTime.UtcNow
            });

            return result;
        }

        public async Task<EmployeeDto> DeleteAsync(int id)
        {
            var deleted=await repository.DeleteAsync(id);
            var result= mapper.Map<EmployeeDto>(deleted);

            await publishEndPoint.Publish(new Events.EmployeeEvent
            {
                EventType = "Deleted",
                EmployeeId = result.Id,
                EmployeeName = result.Name,
                OccurredAt= DateTime.UtcNow

            });
            return result;
        }

        public async Task<List<EmployeeDto>> GetAllAsync()
        {
            //var cached = await cache.GetStringAsync(EmployeeListCacheKey);
            //if (!string.IsNullOrEmpty(cached))
            //{
               
            //    var cachedEmployees = JsonSerializer.Deserialize<List<EmployeeDto>>(cached);
            //    if (cachedEmployees is not null)
            //    {
            //        return cachedEmployees;
            //    }
            //}
            var employees= mapper.Map<List<EmployeeDto>>(await repository.GetAllAsync());
            //await cache.SetStringAsync(EmployeeListCacheKey,
            //    JsonSerializer.Serialize(employees),cacheOptions);
            return employees;
                
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
