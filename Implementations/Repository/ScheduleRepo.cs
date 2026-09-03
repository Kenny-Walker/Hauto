using Hauto.Context;
using Hauto.Entities;
using Hauto.Interface.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hauto.Implementations.Repository
{
    public class ScheduleRepo : GenericRepo<Schedule>, IScheduleRepo
    {
        public ScheduleRepo(HautoContext Context) 
        {
            _Context = Context;
        }

        public async Task<IList<Schedule>> GetAllSchedules()
        {
            return await _Context.Schedules.Include(x => x.Device).OrderByDescending(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<Schedule> GetSchedule(int Id)
        {
            return await _Context.Schedules.Include(x => x.Device).FirstOrDefaultAsync(x => x.Id == Id);
        }
    }
}
