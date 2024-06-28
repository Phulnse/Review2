using Application.IServices;
using Application.ViewModels.HolidayVMs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace WebAPI.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public HolidayService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddHolidays(AddHolidaysReq req)
        {
            req.Holidays = req.Holidays.DistinctBy(x => x.Date).ToList();
            var holidays = _unitOfWork.Holiday.GetHolidays(DateTime.Now.Date).Select(x => x.Date);
            req.Holidays.RemoveAll(x => holidays.Contains(x.Date));

            List<Holiday> newHolidays = new List<Holiday>();
            req.Holidays.ForEach(x =>
            {
                newHolidays.Add(new Holiday
                {
                    Id = Guid.NewGuid(),
                    Date = x.Date,
                });
            });
            
            await _unitOfWork.Holiday.AddHolidays(newHolidays);
            await _unitOfWork.Save();
        }

        public IEnumerable<GetHolidaysRes> GetAllHolidays(DateTime from)
        {
            from = from.Date;
            return _mapper.Map<IEnumerable<GetHolidaysRes>>(_unitOfWork.Holiday.GetHolidays(from));
        }
    }
}
