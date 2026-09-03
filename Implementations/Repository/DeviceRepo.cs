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

        public async Task<IList<Device>> GetAllDevices()
        {
            return await _Context.Devices.OrderByDescending(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<Device> GetDevice(int Id)
        {
            return await _Context.Devices.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IList<Device>> GetActiveDevices(bool isActive)
        {
            return await _Context.Devices.OrderByDescending(x => x.IsDeleted == false && x.isActive == true).ToListAsync();
        }
    }
}
