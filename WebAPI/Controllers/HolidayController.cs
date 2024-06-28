using Application.IServices;
using Application.ViewModels;
using Application.ViewModels.HolidayVMs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/holiday")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly IHolidayService _holidayService;
        public HolidayController(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        [HttpGet]
        public IActionResult Get(DateTime date)
        {
            var result = _holidayService.GetAllHolidays(date);
            return StatusCode(StatusCodes.Status200OK, new Response(200, result));
        }

        [HttpPost]
        public async Task<IActionResult> AddHolidays(IValidator<AddHolidaysReq> validator, AddHolidaysReq req)
        {
            var result = validator.Validate(req);
            if (!result.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new Response(400, "Add Fail"));

            await _holidayService.AddHolidays(req);
            return StatusCode(StatusCodes.Status200OK, new Response(200, "", "Add success"));
        }
    }
}
