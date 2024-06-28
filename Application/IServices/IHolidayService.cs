using Application.ViewModels.HolidayVMs;

namespace Application.IServices
{
    public interface IHolidayService
    {
        Task AddHolidays(AddHolidaysReq req);
        IEnumerable<GetHolidaysRes> GetAllHolidays(DateTime from);
    }
}
