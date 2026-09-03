using Hauto.Entities;

namespace Hauto.Interface.IRepository
{
    public interface IDeviceRepo : IGenericRepo<Device>
    {
        Task<Device> GetDevice(int Id);
        Task<IList<Device>> GetActiveDevices(bool isActive);
        Task<IList<Device>> GetAllDevices();
    }
}
