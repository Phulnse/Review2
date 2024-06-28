using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class HolidayRepository : GenericRepository<Holiday>, IHolidayRepository
    {
        public HolidayRepository(SRMSContext context) : base(context)
        {
        }

        public async Task AddHolidays(IEnumerable<Holiday> holidays)
        {
            await AddBulkAsync(holidays);
        }

        public IEnumerable<Holiday> GetHolidays(DateTime from)
        {
            return Find(x => DateTime.Compare(x.Date, from) > 0).AsEnumerable();
        }
    }
}
