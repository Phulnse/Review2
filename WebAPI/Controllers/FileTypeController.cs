using Application.IServices;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/filetype")]
    [ApiController]
    public class FileTypeController : ControllerBase
    {
        private readonly IFileTypeService _fileTypeService;

        public FileTypeController(IFileTypeService fileTypeService)
        {
            _fileTypeService = fileTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int stateNumber)
        {
            var result = await _fileTypeService.GetFileTypeListAsync(stateNumber);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }
    }
}
