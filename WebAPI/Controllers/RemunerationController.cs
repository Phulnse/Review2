using Application.IServices;
using Application.ViewModels;
using Application.ViewModels.RemunerationVMs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/remuneration")]
    [ApiController]
    public class RemunerationController : ControllerBase
    {
        private readonly IRemunerationService _remunerationService;

        public RemunerationController(IRemunerationService remunerationService)
        {
            _remunerationService = remunerationService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("submit")]
        public async Task<IActionResult> Create(IValidator<SubmitRemunerationReq> validator, SubmitRemunerationReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Submit fail"));

            await _remunerationService.SubmitRemunerationAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Submit success"));
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("censorship")]
        public async Task<IActionResult> Censorship(IValidator<CensorshipRemunerationReq> validator, CensorshipRemunerationReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Censorship fail"));

            await _remunerationService.CensorshipRemuneration(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Censorship success"));
        }

        [Authorize(Roles = "Staff")]
        [HttpGet]
        public async Task<IActionResult> Get(IValidator<GetRemunerationReq> validator,[FromQuery] GetRemunerationReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Get fail"));

            var remuneration = _remunerationService.GetRemunerationOfTopic(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, remuneration));
        }
    }
}
