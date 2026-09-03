using Hauto.Entities;

namespace Hauto.Models.DTOs
{
    public class CreateScheduleDto
    {
        public string ScheduleName { get; set; }
        public DateTime Time { get; set; }
    }

    public class AddScheduledDeviceDto
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
    }
    public class UpdateScheduleDto
    {
        public int Id { get; set; }
        public string ScheduleName { get; set; }
        public DateTime Time { get; set; }
    }
    public class UpdateScheduleStatusDto
    {
        public int Id { get; set; }
        public bool OnRepeat { get; set; }
    }

    public class GetScheduleDto
    {
        public int Id { get; set; }
        public string ScheduleName { get; set; }
        public GetDeviceDto Device { get; set; }
        public int DeviceId { get; set; }
        public DateTime Time { get; set; }
        public bool OnRepeat { get; set; }
    }
    public class ScheduleResponse : BaseResponse
    {
        public List<GetScheduleDto> Data { get; set; } = new List<GetScheduleDto>();
    }
    public class SingleScheduleResponse : BaseResponse
    {
        public GetScheduleDto Data { get; set; } = new GetScheduleDto();
    }
}
