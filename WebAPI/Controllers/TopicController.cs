using Application.IServices;
using Application.Utils;
using Application.ViewModels;
using Application.ViewModels.CouncilVMs;
using Application.ViewModels.TopicVMs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/topic")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("create")]
        public async Task<IActionResult> Post(IValidator<CreateTopicReq> validator, CreateTopicReq createTopicReq)
        {
            var result = await validator.ValidateAsync(createTopicReq);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Create fail"));

            await _topicService.CreateTopicAsync(createTopicReq);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Create success"));
        }

        [Authorize(Roles = "Dean")]
        [HttpGet("topic-for-dean")]
        public async Task<IActionResult> GetTopicForDean(Guid deanId)
        {

            var result = await _topicService.GetTopicsForDeanAsync(deanId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Dean")]
        [HttpGet("topic-decided-by-dean")]
        public async Task<IActionResult> GetTopicDecidedByDean(Guid deanId)
        {
            var result = await _topicService.GetTopicDecidedByDeanAsync(deanId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("pre-topic-waiting-review-formation")]
        public async Task<IActionResult> GetTopicWaitingForReviewFormation()
        {
            var progressInState = TopicUtil.GetProgressInState(2);
            var result = await _topicService.GetTopicByStateAndProgressForStaffAsync(progressInState.state, progressInState.progress);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("early-topic-waiting-council-formation")]
        public async Task<IActionResult> GetTopicWaitingForCouncilFormation()
        {
            var progressInState = TopicUtil.GetProgressInState(4);
            var result = await _topicService.GetTopicByStateAndProgressForStaffAsync(progressInState.state, progressInState.progress);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("all-topic-waiting-upload-meeting-minutes")]
        public async Task<IActionResult> GetTopicWaitingForCouncilMeeting()
        {
            var result = await _topicService.GetTopicWaitingUploadMeetingMinutesAsync();
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "User")]
        [HttpGet("pre-topic-waiting-reviewer")]
        public async Task<IActionResult> GetTopicForReviewer(Guid memberId)
        {
            var result = await _topicService.GetTopicsForReviewMemberAsync(memberId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("pre-topic-waiting-review-for-staff")]
        public async Task<IActionResult> GetTopicForReview()
        {
            var progressInState = TopicUtil.GetProgressInState(3);
            var result = await _topicService.GetTopicByStateAndProgressForStaffAsync(progressInState.state, progressInState.progress);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "User")]
        [HttpGet("topic-for-user")]
        public async Task<IActionResult> GetTopicForUser(Guid userId)
        {
            var result = await _topicService.GetTopicByUserIdAsync(userId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("early-topic-waiting-resubmit")]
        public async Task<IActionResult> GetEarlyTopicWaitingResubmit()
        {
            var result = await _topicService.GetEarlyTopicWaitingResubmitAsync();
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [HttpGet("early-topic-waiting-chairman-decision")]
        public async Task<IActionResult> GetEarlyTopicWaitingChairmanDecision(Guid councilId)
        {
            var result = await _topicService.GetTopicWaitingChairmanDecisionAsync(councilId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("early-topic-waiting-upload-contract")]
        public async Task<IActionResult> GetEarlyTopicWaitingForUploadContract()
        {
            var progressInState = TopicUtil.GetProgressInState(7);
            var result = await _topicService.GetTopicByStateAndProgressForStaffAsync(progressInState.state, progressInState.progress);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize]
        [HttpGet("all-documents")]
        public async Task<IActionResult> GetTopicWithDocument(Guid topicId)
        {
            var result = await _topicService.GetDocumentOfTopicAsync(topicId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "User")]
        [HttpGet("documents-for-council")]
        public async Task<IActionResult> GetTopicWithDocumentForCouncil(Guid councilId, Guid topicId)
        {
            var result = await _topicService.GetEarlyTopicDetailForCouncilAsync(councilId, topicId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [HttpPost("dean-make-decision")]
        public async Task<IActionResult> DeanMakeDecision(IValidator<DeanMakeDecisionReq> validator, DeanMakeDecisionReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Make decision fail"));

            await _topicService.DeanMakeDecisionAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Make decision success"));
        }

        [Authorize(Roles = "User")]
        [HttpPost("chairman-approve")]
        public async Task<IActionResult> ChairmanApprove(Guid topicId)
        {
            await _topicService.ChairmanApproveAsync(topicId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Approved"));
        }

        [Authorize(Roles = "User")]
        [HttpPost("chairman-reject")]
        public async Task<IActionResult> ChairmanReject(ChairmanRejectReq req)
        {
            await _topicService.ChairmanRejectAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Rejected"));
        }

        [Authorize(Roles = "User")]
        [HttpPost("chairman-make-final-decision")]
        public async Task<IActionResult> ChairmanMakeFinalDecision(IValidator<ChairmanMakeFinalDecisionReq> validator, ChairmanMakeFinalDecisionReq req)
        {
            var result = await validator.ValidateAsync(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Make decision fail"));

            await _topicService.ChairmanMakeFinalDecisionAsync(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Make decision success"));
        }

        [Authorize]
        [HttpGet("detail")]
        public async Task<IActionResult> GetTopicDetail(Guid topicId)
        {
            var result = await _topicService.GetTopicDetailAsync(topicId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize]
        [HttpGet("process")]
        public async Task<IActionResult> GetTopicProcess(Guid topicId)
        {
            var result = await _topicService.GetTopicProcessAsync(topicId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "User")]
        [HttpGet("ongoing-topic-for-council")]
        public async Task<IActionResult> GetOngoningTopicForCouncil(Guid councilId)
        {
            var result = await _topicService.GetOngoingTopicForCouncilAsync(councilId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "User")]
        [HttpGet("topic-reviewed-for-member")]
        public async Task<IActionResult> GetTopicReviewedForMember(Guid memberId)
        {
            var result = await _topicService.GetTopicReviewedForMemberAsync(memberId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "User")]
        [HttpGet("meeting-infor")]
        public async Task<IActionResult> GetMeetingInfor(Guid userId, Guid topicId)
        {
            var result = await _topicService.GetTopicMeetingInforResAsync(userId, topicId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("active-topic")]
        public async Task<IActionResult> GetActiveTopics()
        {
            var result = await _topicService.GetAllActiveTopicAsync();
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "User")]
        [HttpGet("all-review-documents")]
        public async Task<IActionResult> GetAllReviewDocuments(Guid userId, Guid topicId)
        {
            var result = await _topicService.GetDocumentsOfTopicAsync(userId, topicId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("move-to-middle-term")]
        public async Task<IActionResult> MoveTopicToMiddleTerm(Guid topicId)
        {
            await _topicService.MoveTopicStateToMiddleTermAsync(topicId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "Move success"));
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("move-to-final-term")]
        public async Task<IActionResult> MoveTopicToFinalTerm(Guid topicId)
        {
            await _topicService.MoveTopicStateToFinalTermAsync(topicId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "Move success"));
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("middle-topic-waiting-make-schedule")]
        public async Task<IActionResult> GetMiddleTopicWaitingConfig()
        {
            var progressInState = TopicUtil.GetProgressInState(10);
            var result = await _topicService.GetTopicByStateAndProgressForStaffAsync(progressInState.state, progressInState.progress);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("middle-topic-waiting-configure-conference")]
        public async Task<IActionResult> GetMiddleTopicHaveBeenDocument()
        {
            var progressInState = TopicUtil.GetProgressInState(12);
            var result = await _topicService.GetTopicByStateAndProgressForStaffAsync(progressInState.state, progressInState.progress);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("final-topic-waiting-configure-conference")]
        public async Task<IActionResult> GetFinalTopicWaitingConfig()
        {
            var progressInState = TopicUtil.GetProgressInState(17);
            var result = await _topicService.GetTopicByStateAndProgressForStaffAsync(progressInState.state, progressInState.progress);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "User")]
        [HttpGet("topic-has-been-resolved-for-council")]
        public async Task<IActionResult> GetTopicHasBeenResolved(Guid councilId)
        {
            var result = await _topicService.GetTopicHasBeenResolvedForCouncilAsync(councilId);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("final-topic-waiting-make-schedule")]
        public async Task<IActionResult> GetFinalTopicWaitingMakeSchedule()
        {
            var progressInState = TopicUtil.GetProgressInState(15);
            var result = await _topicService.GetTopicByStateAndProgressForStaffAsync(progressInState.state, progressInState.progress);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("final-topic-waiting-censorship-remuneration")]
        public async Task<IActionResult> GetFinalTopicWaitingCensorshipRemuneration()
        {
            var progressInState = TopicUtil.GetProgressInState(22);
            var result = await _topicService.GetTopicByStateAndProgressForStaffAsync(progressInState.state, progressInState.progress);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTopic()
        {
            var result = await _topicService.GetAllTopicAsync();
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }
    }
}
