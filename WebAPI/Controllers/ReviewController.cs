using Application.IServices;
using Application.ViewModels;
using Application.ViewModels.ReviewVMs;
using Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("config-early")]
        public async Task<IActionResult> ConfigEarlyReview(IValidator<ConfigEarlyReviewReq> validator, ConfigEarlyReviewReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Config early review fail"));

            var configureResult = await _reviewService.ConfigEarlyReviewAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, configureResult, "Config early review success"));
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("config-middle")]
        public async Task<IActionResult> ConfigMiddleReview(IValidator<ConfigMiddleReviewReq> validator, ConfigMiddleReviewReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Config middle review fail"));

            var configureResult = await _reviewService.ConfigMiddleReviewAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, configureResult, "Config middle review success"));
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("config-final")]
        public async Task<IActionResult> ConfigFinalReview(IValidator<ConfigFinalReviewReq> validator, ConfigFinalReviewReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Config final review fail"));

            var configResult = await _reviewService.ConfigFinalReviewAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, configResult, "Config final review success"));
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("update-early-meeting-result")]
        public async Task<IActionResult> UpdateEarlyMeetingResult(IValidator<UploadEarlyMeetingMinutesReq> validator, UploadEarlyMeetingMinutesReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Update fail"));

            await _reviewService.UpdateEarlyMeetingResultAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Update complete"));
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("update-final-meeting-result")]
        public async Task<IActionResult> UpdateFinalMeetingResult(IValidator<UploadFinalMeetingMinutesReq> validator, UploadFinalMeetingMinutesReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Update fail"));

            await _reviewService.UpdateFinalMeetingResultAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Update complete"));
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("edit-deadline-for-early-review")]
        public async Task<IActionResult> EditDeadlineForEarlyReview(Guid topicId, DateTime deadline)
        {
            await _reviewService.EditDeadlineAsync(topicId, ReviewStateEnum.EarlyTermReport, deadline);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Edit success"));
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("make-middle-review-schedule")]
        public async Task<IActionResult> MakeMiddleReviewSchedule(IValidator<MakeMiddleReviewScheduleReq> validator, MakeMiddleReviewScheduleReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Make middle schedule fail"));

            await _reviewService.MakeMiddleReviewScheduleAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Make middle schedule success"));
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("make-final-review-schedule")]
        public async Task<IActionResult> MakeFinalReviewSchedule(IValidator<MakeFinalReviewScheduleReq> validator, MakeFinalReviewScheduleReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Make final schedule fail"));

            await _reviewService.MakeFinalReviewScheduleAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Make final schedule success"));
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("create-evaluate")]
        public async Task<IActionResult> CreateEvaluate(IValidator<UploadEvaluateReq> validator, UploadEvaluateReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Upload evaluate fail"));

            await _reviewService.UploadEvaluateAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Upload evaluate success"));
        }
    }
}
