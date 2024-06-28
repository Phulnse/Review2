using Application.IServices;
using Application.ViewModels;
using Application.ViewModels.DocumentVMs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/document")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("resubmit-early-document")]
        public async Task<IActionResult> ResubmitEarlyDocument(IValidator<ResubmitEarlyDocumentReq> validator, ResubmitEarlyDocumentReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Upload early document fail"));

            await _documentService.ResubmitDocumentForEarlyReviewAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Upload early document success"));
        }

        [Authorize(Roles = "User")]
        [HttpPost("supplementation-middle-document")]
        public async Task<IActionResult> SupplementationMiddleDocument(IValidator<SupplementationMiddleDocumentReq> validator,SupplementationMiddleDocumentReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Upload middle document fail"));

            await _documentService.SupplementationDocumentForMiddleReviewAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Upload middle document success"));
        }

        [Authorize(Roles = "User")]
        [HttpPost("supplementation-final-document")]
        public async Task<IActionResult> SupplementationFinalDocument(IValidator<SupplementationFinalDocumentReq> validator, SupplementationFinalDocumentReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Upload final document fail"));

            await _documentService.SupplementationDocumentForFinalReviewAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Upload final document success"));
        }

        [Authorize(Roles = "User")]
        [HttpPost("resubmit-final-document")]
        public async Task<IActionResult> ResubmitFinalDocument(IValidator<ResubmitFinalDocumentReq> validator, ResubmitFinalDocumentReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Resubmit final document fail"));

            await _documentService.ResubmitDocumentForFinalReviewAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Resubmit final document success"));
        }
    }
}
