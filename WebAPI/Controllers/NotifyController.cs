using Application.IServices;
using Application.ViewModels;
using Application.ViewModels.NotifyVMs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/notify")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly INotifyService _notifierService;
        public NotifyController(INotifyService notifyService)
        {
            _notifierService = notifyService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] GetNotifyReq req)
        {
            var result = _notifierService.GetNotifiesOfTopicForOwner(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }
    }
}
