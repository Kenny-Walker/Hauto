using Hauto.Context;
using Hauto.Entities;
using Hauto.Interface.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hauto.Implementations.Repository
{
    public class ScheduledDeviceRepo : GenericRepo<ScheduledDevice>, IScheduledDeviceRepo
    {
        public ScheduledDeviceRepo(HautoContext Context)
        {
            _Context = Context;
        }

        public async Task<ScheduledDevice> GetScheduledDevice(int DeviceId)
        {
            return await _Context.ScheduledDevices.Include(x => x.Schedule).Include(x => x.Device).FirstOrDefaultAsync(x => x.DeviceId == DeviceId && x.Schedule.IsDeleted == false);
        }

        public async Task<IList<ScheduledDevice>> GetScheduledDevices(int scheduleId)
        {
            return await _Context.ScheduledDevices.Include(x => x.Schedule).Include(x => x.Device).OrderByDescending(x => x.ScheduleId == scheduleId && x.IsDeleted == false && x.Schedule.IsDeleted == false).ToListAsync();
        }
    }
}
