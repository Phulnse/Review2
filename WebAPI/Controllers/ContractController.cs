using Application.IServices;
using Application.ViewModels;
using Application.ViewModels.ContractVMs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/contract")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractService _contractService;
        public ContractController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("upload-early-contract")]
        public async Task<IActionResult> Post(IValidator<UploadEarlyContractReq> validator, UploadEarlyContractReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "", "Upload early fail"));
            await _contractService.UploadEarlyContractAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Upload early success"));
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("upload-ending-contract")]
        public async Task<IActionResult> UploadEndingContract(IValidator<UploadContractForEndingPhaseReq> validator, UploadContractForEndingPhaseReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "", "Upload ending contract fail"));
            await _contractService.UploadContractForEndingPhaseAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Upload ending contract success"));
        }
    }
}
