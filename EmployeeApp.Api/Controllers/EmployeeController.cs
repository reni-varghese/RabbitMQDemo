using EmployeeApp.Api.Exceptions;
using EmployeeApp.Api.Models.Dtos;
using EmployeeApp.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApp.Api.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController(IEmployeeService service) : ControllerBase
    {
        [HttpGet]
        //[AllowAnonymous]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles ="User,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await service.GetAllAsync();
            //throw new ArgumentNullException("Something went wrong. Please try after sometime");
            return Ok(result);
        }
        [HttpGet("{id}")]
        //[AllowAnonymous]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User,Admin")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await service.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);

        }
        [HttpPost]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles ="Admin")]
        public async Task<IActionResult> Create([FromBody] EmployeeDto entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();

            }
            var result=await service.AddAsync(entity);
            if (result is null) return NotFound();
            return CreatedAtAction("GetById", new { id = result.Id }, result);

        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,User")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeDto entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await service.UpdateAsync(id, entity);
            if (result is null) return NotFound();
            return Ok(result);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result=await service.DeleteAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }


    }
}
