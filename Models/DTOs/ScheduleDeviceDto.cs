namespace Hauto.Models.DTOs
{
    public class UpdateScheduleDeviceStatusDto
    {
        public int ScheduleId { get; set; }
        public int DeviceId { get; set; }
        public bool isActive { get; set; }
    }
    public class GetScheduleDeviceDto
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int ScheduleId { get; set; }
        public GetScheduleDto Schedule { get; set; }
        public GetDeviceDto Device { get; set; }
        public bool isActive { get; set; }
    }
    public class ScheduleDeviceResponse : BaseResponse
    {
        public List<GetScheduleDeviceDto> Data { get; set; } = new List<GetScheduleDeviceDto>();
    }
    public class SingleScheduleDeviceResponse : BaseResponse
    {
        public GetScheduleDeviceDto Data { get; set; } = new GetScheduleDeviceDto();
    }
}
