using Hauto.Entities;
using Hauto.Interface.IRepository;
using Hauto.Interface.IService;
using Hauto.Models.DTOs;

namespace Hauto.Implementations.Service
{
    public class DeviceService : IDeviceService
    {
        IDeviceRepo _DeviceRepository;
        ILogRepo _logRepository;
        public DeviceService(IDeviceRepo DeviceRepository, ILogRepo logRepository)
        {
            _DeviceRepository = DeviceRepository;
            _logRepository = logRepository;
        }

        public async Task<BaseResponse> CreateDevice(CreateDeviceDto createDevice)
        {
            var device = new Device()
            {
                DeviceName = createDevice.DeviceName,
                isActive = false
            };
            await _DeviceRepository.CreateAsync(device);
            var log = new Log()
            {
                DeviceId = device.Id,
                LogStatement = $"{device.DeviceName} Created",
                Date = DateTime.UtcNow,
            };
            await _logRepository.CreateAsync(log);
            return new BaseResponse()
            {
                Message = "Device Added Succesfully",
                Success = true
            };
        }

        public async Task<DeviceResponse> GetAllDevices()
        {
            var Device = await _DeviceRepository.GetAllDevices();
            if (Device == null)
            {
                return new DeviceResponse()
                {
                    Data = null,
                    Message = "Couldn't Retrieve Devices",
                    Success = false
                };
            }
            var DevicesList = new List<GetDeviceDto>();
            foreach (var x in Device) DevicesList.Add(GetDeviceDetails(x));
            return new DeviceResponse
            {
                Data = DevicesList,
                Success = true,
                Message = "Devices Retrieved Successfully"
            };
        }

        public async Task<BaseResponse> GetDevice(int Id)
        {
            var Device = await _DeviceRepository.GetDevice(Id);
            if (Device == null)
            {
                return new SingleDeviceResponse()
                {
                    Data = null,
                    Message = "Device does not exist",
                    Success = false
                };
            }
            return new SingleDeviceResponse()
            {
                Data = GetDeviceDetails(Device),
                Message = "Device Retrieved",
                Success = true
            };
        }

        public GetDeviceDto GetDeviceDetails(Device x)
        {
            return new GetDeviceDto()
            {
                Id = x.Id,
                DeviceName = x.DeviceName,
                isActive = x.isActive
            };
        }

        public async Task<BaseResponse> UpdateDevice(UpdateDeviceDto updateDevice)
        {
            var Device = await _DeviceRepository.GetDevice(updateDevice.DeviceId);
            if (Device == null)
            {
                return new BaseResponse()
                {
                    Message = "Device does not exist",
                    Success = false
                };
            }
            Device.DeviceName = updateDevice.DeviceName;
            await _DeviceRepository.UpdateAsync(Device);
            var log = new Log()
            {
                DeviceId = Device.Id,
                LogStatement = $"{Device.DeviceName} Updated",
                Date = DateTime.UtcNow,
            };
            await _logRepository.CreateAsync(log);
            return new BaseResponse()
            {
                Message = "Device Updated",
                Success = true
            };
        }

        public async Task<BaseResponse> UpdateDeviceStatus(UpdateDeviceStatusDto updateDeviceStatus)
        {
            var Device = await _DeviceRepository.GetDevice(updateDeviceStatus.DeviceId);
            if (Device == null)
            {
                return new BaseResponse()
                {
                    Message = "Device does not exist",
                    Success = false
                };
            }
            Device.isActive = updateDeviceStatus.isActive;
            await _DeviceRepository.UpdateAsync(Device);
            if (Device.isActive == true)
            {
                var log = new Log()
                {
                    DeviceId = Device.Id,
                    LogStatement = $"{Device.DeviceName} is On",
                    Date = DateTime.UtcNow,
                };
                await _logRepository.CreateAsync(log);
            }
            var logOff = new Log()
            {
                DeviceId = Device.Id,
                LogStatement = $"{Device.DeviceName} is Off",
                Date = DateTime.UtcNow,
            };
            await _logRepository.CreateAsync(logOff);
            return new BaseResponse()
            {
                Message = "Device Status Updated",
                Success = true
            };
        }
    }
}
