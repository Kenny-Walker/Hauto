using Hauto.Entities;
using Hauto.Interface.IRepository;
using Hauto.Interface.IService;
using Hauto.Models.DTOs;

namespace Hauto.Implementations.Service
{
    public class HubsService : IHubsService
    {
        IDeviceRepo _DeviceRepository;
        ILogRepo _logRepository;
        IHubsRepo _hubRepository;
        public HubsService(IDeviceRepo DeviceRepository, ILogRepo logRepository, IHubsRepo hubRepository)
        {
            _DeviceRepository = DeviceRepository;
            _logRepository = logRepository;
            _hubRepository = hubRepository;
        }

        public async Task<BaseResponse> CreateHub(CreateHubsDto createHub)
        {
            var hub = new Hubs()
            {
                HubName = createHub.HubName,
            };
            await _hubRepository.CreateAsync(hub);
            return new BaseResponse()
            {
                Message = "Hub Created Successfully",
                Success = true,
            };
        }

        public async Task<HubsResponse> GetAllHubs()
        {
            var hub = await _hubRepository.GetAllHubs();
            if (hub == null)
            {
                return new HubsResponse()
                {
                    Data = null,
                    Message = "Couldn't Retrieve Hubs",
                    Success = false
                };
            }
            var HubList = new List<GetHubsDto>();
            foreach (var x in hub) HubList.Add(GetHubDetails(x));
            return new HubsResponse
            {
                Data = HubList,
                Success = true,
                Message = "Hubs Retrieved Successfully"
            };
        }

        public async Task<BaseResponse> GetHub(int Id)
        {
            var hub = await _hubRepository.GetHub(Id);
            if (hub == null)
            {
                return new SingleHubResponse()
                {
                    Data = null,
                    Message = "Hub doesn't exist",
                    Success = false
                };
            }
            return new SingleHubResponse()
            {
                Data = GetHubDetails(hub),
                Message = "Hub Retrived Successfully",
                Success = true
            };
        }

        public async Task<BaseResponse> UpdateHub(UpdateHubsDto updateHub)
        {
            var hub = await _hubRepository.GetHub(updateHub.Id);
            if (hub == null)
            {
                return new BaseResponse()
                {
                    Message = "Hub doesn' exist",
                    Success = false
                };
            }
            hub.HubName = updateHub.HubName;
            await _hubRepository.UpdateAsync(hub);
            return new BaseResponse()
            {
                Message = "Hub Updated Successfully",
                Success = true
            };

        }
        public GetHubsDto GetHubDetails(Hubs x)
        {
            return new GetHubsDto()
            {
                Id = x.Id,
                HubName = x.HubName,
                Devices = x.Devices.Select(c => new GetDeviceDto()
                {
                    Id = c.Id,
                    HubId = c.HubId,
                    DeviceName = c.DeviceName,
                    isActive = c.isActive
                }
                ).ToList()
            }; 
        }
    }
}
