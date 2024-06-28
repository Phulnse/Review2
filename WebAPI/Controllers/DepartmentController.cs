using Application.IServices;
using Application.ViewModels;
using Application.ViewModels.DepartmentVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> Get()
        {
            var result = await _departmentService.GetDepartmentsAsync();
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateDepartmentReq req)
        {
            await _departmentService.CreateDepartmentAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Create success"));
        }

        //update
        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateDepartmentReq req)
        {
            var result = await _departmentService.UpdateDepartmentAsync(req);
            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Update success"));
            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "", "Update failed"));
        }

        //delete
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _departmentService.DeleteDepartmentAsync(id);
            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Delete success"));
            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "", "Delete failed"));
        }
    }
}
