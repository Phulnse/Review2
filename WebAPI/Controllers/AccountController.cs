using Application.IServices;
using Application.ViewModels;
using Application.ViewModels.AccountVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInfor login)
        {
            var result = await _accountService.LoginAsync(login);

            if (result != null)
                return StatusCode(StatusCodes.Status200OK, new Response(200, result, "Login success"));

            return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Login fail"));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("inactive-emails")]
        public IActionResult GetInactiveEmails()
        {
            var result = _accountService.GetInactiveEmails();

            return StatusCode(StatusCodes.Status200OK, new Response(200, result, ""));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("provide-account")]
        public async Task<IActionResult> ProvideAccount(ProvideAccountReq req)
        {
            await _accountService.ProvideAccountForUserViaEmail(req.Emails);

            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Success"));
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordReq req)
        {
            var result = await _accountService.ChangePassword(req);
            if (result)
                return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Change password success"));

            return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Change password fail"));
        }

        [Authorize]
        [HttpPut("change-password-with-otp")]
        public async Task<IActionResult> ChangePasswordWithOtp(ChangePasswordWithOtpReq req)
        {
            var result = await _accountService.ChangePassword(req);
            if (result)
                return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Change password success"));

            return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Change password fail"));
        }

        [Authorize]
        [HttpPut("request-otp")]
        public async Task<IActionResult> RequestOtp(AccountVM req)
        {
            await _accountService.RequestOtp(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Request success"));
        }
    }
}
