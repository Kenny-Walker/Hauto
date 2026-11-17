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

        public async Task<IList<Schedule>> GetAllSchedules(int UserId)
        {
            return await _Context.Schedules.Include(s => s.ScheduledDevice).ThenInclude(sd => sd.Device).ThenInclude(yd => yd.Hub).OrderByDescending(x => x.IsDeleted == false && x.UserId == UserId).ToListAsync();
        }

        public async Task<Schedule> GetSchedule(int Id)
        {
            return await _Context.Schedules.Include(s => s.ScheduledDevice).ThenInclude(sd => sd.Device).ThenInclude(yd => yd.Hub).FirstOrDefaultAsync(x => x.Id == Id);
        }
    }
}
