using AutoMapper;
using EmployeeApp.Api.Models;
using EmployeeApp.Api.Models.Dtos;

namespace EmployeeApp.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();

        }
    }
}
