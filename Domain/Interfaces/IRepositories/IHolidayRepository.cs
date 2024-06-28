using Domain.Entities;

namespace Domain.Interfaces.IRepositories
{
    public interface IHolidayRepository
    {
        IEnumerable<Holiday> GetHolidays(DateTime from);
        Task AddHolidays(IEnumerable<Holiday> holidays);
    }
}
