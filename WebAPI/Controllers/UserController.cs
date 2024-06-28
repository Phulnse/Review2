using Application.IServices;
using Application.ViewModels;
using Application.ViewModels.UserVMs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> Get()
        {
            var result = await _userService.GetAllAsync();
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<IActionResult> GetNormalUser(Guid userId)
        {
            var result = await _userService.GetUsersByRole(false, userId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "User, Staff")]
        [HttpGet("users-not-participating-topic")]
        public async Task<IActionResult> GetUsersNotParcitipatingTopic(Guid topicId)
        {
            var result = await _userService.GetUsersNotParticipatingTopicAsync(topicId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assign-dean")]
        public async Task<IActionResult> AssignDean(IValidator<AssignDeanReq> validator, AssignDeanReq req)
        {
            var result = validator.Validate(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Assign fail"));

            await _userService.AssignDeanAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Assign success"));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-users")]
        public async Task<IActionResult> CreateUsers(CreateUserDataReq req)
        {
            await _userService.CreateUserDataAsync(req.Users);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Create success"));
        }

        // CRUD 
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userService.GetByIdAsync(id);
            if (result == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response(404, "", "User not found"));
            }
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateUserDataReq req)
        {
            await _userService.CreateUserDataAsync(req.Users);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Create success"));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateUserReq req)
        {
            var result = await _userService.UpdateUserAsync(req);
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
            var result = await _userService.DeleteUserAsync(id);
            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Delete success"));
            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "", "Delete failed"));
        }
    }
}
