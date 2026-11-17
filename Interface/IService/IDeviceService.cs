using Hauto.Models.DTOs;

namespace Hauto.Interface.IService
{
    public interface IDeviceService
    {
        Task<BaseResponse> CreateDevice(CreateDeviceDto createDevice);
        Task<BaseResponse> UpdateDevice(UpdateDeviceDto updateDevice);
        Task<BaseResponse> UpdateDeviceStatus(UpdateDeviceStatusDto updateDeviceStatus);
        Task<BaseResponse> GetDevice(int Id);
        Task<DeviceResponse> GetAllDevicesByHubId(int HubId);
        Task<DeviceResponse> GetAllDevices(int UserId);
        Task<BaseResponse> KillSwitchProtocol(int UserId, string Passcode);
    }
}
