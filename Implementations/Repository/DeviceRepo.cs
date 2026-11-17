using Hauto.Context;
using Hauto.Entities;
using Hauto.Interface.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hauto.Implementations.Repository
{
    public class DeviceRepo : GenericRepo<Device>, IDeviceRepo
    {
        public DeviceRepo(HautoContext Context) 
        {
            _Context = Context;
        }

        public async Task<IList<Device>> GetAllDevices(int HubId)
        {
            return await _Context.Devices.Include(x => x.Hub).OrderByDescending(x => x.IsDeleted == false && x.HubId == HubId).ToListAsync();
        }
        public async Task<IList<Device>> GetAllUserDevices(int UserId)
        {
            return await _Context.Devices.Include(x => x.Hub).OrderByDescending(x => x.IsDeleted == false && x.UserId == UserId).ToListAsync();
        }
        public async Task<Device> GetDevice(int Id)
        {
            return await _Context.Devices.Include(x => x.Hub).FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IList<Device>> GetActiveDevices(bool isActive)
        {
            return await _Context.Devices.Include(x => x.Hub).OrderByDescending(x => x.IsDeleted == false && x.isActive == true).ToListAsync();
        }
    }
}
