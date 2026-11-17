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
        IMqttService _mqttService;
        IHubsRepo _hubRepository;
        IUserRepo _userRepository;
        public DeviceService(IDeviceRepo DeviceRepository, ILogRepo logRepository, IMqttService mqttService, IHubsRepo hubRepository, IUserRepo userRepository)
        {
            _DeviceRepository = DeviceRepository;
            _logRepository = logRepository;
            _mqttService = mqttService;
            _hubRepository = hubRepository;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse> CreateDevice(CreateDeviceDto createDevice)
        {
            var hub = await _hubRepository.GetHub(createDevice.HubId);
            var user = await _userRepository.GetUserId(createDevice.UserId);
            if (hub != null || user != null) 
            {
                var device = new Device()
                {
                    UserId = createDevice.UserId,
                    HubId = createDevice.HubId,
                    DeviceName = createDevice.DeviceName,
                    isActive = false
                };
                await _DeviceRepository.CreateAsync(device);
                var log = new Log()
                {
                    UserId = device.UserId,
                    DeviceId = device.Id,
                    LogStatement = $"{device.DeviceName} Created in {hub.HubName}",
                    Date = DateTime.UtcNow,
                };
                await _logRepository.CreateAsync(log);
                return new BaseResponse()
                {
                    Message = "Device Added Succesfully",
                    Success = true
                };
            }
            return new BaseResponse()
            {
                Message = "Failed to Add Device",
                Success = true
            };
        }

        public async Task<DeviceResponse> GetAllDevicesByHubId(int HubId)
        {
            var Device = await _DeviceRepository.GetAllDevices(HubId);
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
                UserId = x.UserId,
                HubId = x.HubId,
                Hub = new GetHubsDto()
                {
                    Id = x.Hub.Id,
                    HubName = x.Hub.HubName,
                },
                DeviceName = x.DeviceName,
                isActive = x.isActive
            };
        }

        public async Task<DeviceResponse> GetAllDevices(int UserId)
        {
            var Device = await _DeviceRepository.GetAllDevices(UserId);
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
                UserId = Device.UserId,
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
            await _mqttService.PublishCommand(GetDeviceDetails(Device));
            if (Device.isActive == true)
            {
                var log = new Log()
                {
                    UserId = Device.UserId,
                    DeviceId = Device.Id,
                    LogStatement = $"{Device.DeviceName} is On",
                    Date = DateTime.UtcNow,
                };
                await _logRepository.CreateAsync(log);
            }
            var logOff = new Log()
            {
                UserId = Device.UserId,
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

        public async Task<BaseResponse> KillSwitchProtocol(int UserId, string Passcode)
        {
            var devices = await _DeviceRepository.GetAllDevices(UserId);
            var user = await _userRepository.GetUserId(UserId);
            if (devices == null || user == null)
            {
                return new BaseResponse()
                {
                    Message = "No devices found",
                    Success = false
                };
            }
            if (user != null && user.Passcode == Passcode)
            {
                foreach (var device in devices)
                {
                    device.isActive = false;
                    await _DeviceRepository.UpdateAsync(device);
                }
                return new BaseResponse()
                {
                    Message = "Kill Switch Protocol Activated",
                    Success = true
                };
            }
            return new BaseResponse()
            {
                Message = "Failed to Initiate Kill Switch Protocol",
                Success = false
            };


        }
        /*public async Task SyncDevices()
{
   var devices = await GetAllDevices();
   await _mqttService.PublishSync(devices.Data);
}*/
    }
}
