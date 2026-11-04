using Hauto.Entities;

namespace Hauto.Interface.IRepository
{
    public interface IScheduledDeviceRepo : IGenericRepo<ScheduledDevice>
    {
        Task<IList<ScheduledDevice>> GetScheduledDevices(int scheduleId);
        Task<ScheduledDevice> GetScheduledDevice(int DeviceId);
    }
}
