namespace Hauto.Models.DTOs
{
    public class CreateHubsDto
    {
        public int UserId { get; set; }
        public string HubName { get; set; }
    }
    public class UpdateHubsDto
    {
        public int Id { get; set; }
        public string HubName { get; set; }
    }
    public class GetHubsDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string HubName { get; set; }
        public List<GetDeviceDto> Devices { get; set; }
    }

    public class HubsResponse : BaseResponse
    {
        public List<GetHubsDto> Data { get; set; } = new List<GetHubsDto>();
    }
    public class SingleHubResponse : BaseResponse
    {
        public GetHubsDto Data { get; set; } = new GetHubsDto();
    }
}
