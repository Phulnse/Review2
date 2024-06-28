using Application.IServices;
using Application.ViewModels;
using Application.ViewModels.CategoryVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCategoryReq req)
        {
            var result = await _categoryService.CreateCategoryAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result, "Create success"));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateCategoryReq req)
        {
            var result = await _categoryService.UpdateCategoryAsync(req);
            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Update success"));
            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "", "Update failed"));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Delete success"));
            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "", "Delete failed"));
        }
    }
}
