using Application.IServices;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [Authorize]
        [HttpPost("single-DO"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFileDO(IFormFile formFile)
        {
            try
            {
                var result = await _fileService.UploadFileToDOAsync(formFile);
                return StatusCode(StatusCodes.Status200OK, new Response(200, result, "Upload complete"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(500, ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("convert-excel-to-user"), DisableRequestSizeLimit]
        public async Task<IActionResult> Import(IFormFile formFile)
        {
            try
            {
                var result = await _fileService.ConvertExcelFileToUser(formFile);
                return StatusCode(StatusCodes.Status200OK, new Response(200, result, "Convert complete"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response(500, ex.Message));
            }
        }
    }
}
