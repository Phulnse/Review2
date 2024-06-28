using Application.IServices;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/contracttype")]
    [ApiController]
    public class ContractTypeController : ControllerBase
    {
        private readonly IContractTypeService _contractTypeService;

        public ContractTypeController(IContractTypeService contractTypeService)
        {
            _contractTypeService = contractTypeService;
        }

        [Authorize(Roles = "Staff")]
        [HttpGet]
        public async Task<IActionResult> Get(int contractTypeSateNumber)
        {
            var result = await _contractTypeService.GetContractTypeByStateAsync(contractTypeSateNumber);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }
    }
}
