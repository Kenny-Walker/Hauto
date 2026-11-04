using Hauto.Models.DTOs;

namespace Hauto.Interface.IService
{
    public interface IHubsService
    {
        Task<BaseResponse> CreateHub(CreateHubsDto createHub);
        Task<BaseResponse> UpdateHub(UpdateHubsDto updateHub);
        Task<BaseResponse> GetHub(int Id);
        Task<HubsResponse> GetAllHubs();
    }
}
