using Application.IServices;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/nation")]
    [ApiController]
    public class NationController : ControllerBase
    {
        private readonly INationService _nationService;
        public NationController(INationService nationService)
        {
            _nationService = nationService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _nationService.GetAll();
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }
    }
}
