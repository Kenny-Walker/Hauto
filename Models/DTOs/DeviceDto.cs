namespace Hauto.Models.DTOs
{
    public class CreateDeviceDto
    {
        public string DeviceName { get; set; }
    }

    public class UpdateDeviceDto
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
    }

    public class UpdateDeviceStatusDto
    {
        public int DeviceId { get; set; }
        public bool isActive { get; set; }
    }

    public class GetDeviceDto
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public bool isActive { get; set; }
    }

    public class DeviceResponse : BaseResponse
    {
        public List<GetDeviceDto> Data { get; set; } = new List<GetDeviceDto>();
    }
    public class SingleDeviceResponse : BaseResponse
    {
        public GetDeviceDto Data { get; set; } = new GetDeviceDto();
    }
}
