using Application.IServices;
using Application.ViewModels;
using Application.ViewModels.MemberReviewVMs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/memberreview")]
    [ApiController]
    public class MemberReviewController : ControllerBase
    {
        private readonly IMemberReviewService _memberReviewService;

        public MemberReviewController(IMemberReviewService memberReviewService)
        {
            _memberReviewService = memberReviewService;
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("add-reviewer")]
        public async Task<IActionResult> AddMemberReview(IValidator<AddMemberReviewReq> validator, AddMemberReviewReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Add reviewer fail"));

            await _memberReviewService.AddMemberReviewAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Add reviewer success"));           
        }

        [Authorize(Roles = "User")]
        [HttpPost("make-decision")]
        public async Task<IActionResult> MakeDecision(IValidator<MemberReviewMakeDecisionReq> validator, MemberReviewMakeDecisionReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Make decision fail"));

            await _memberReviewService.MemberReviewMakeDecisionAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Make decision success"));
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("member-review-of-topic")]
        public async Task<IActionResult> GetMemberReviewOfTopic(Guid topicId)
        {
            var result = await _memberReviewService.GetMemberReviewOfTopicAsync(topicId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }
    }
}
