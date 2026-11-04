using Hauto.Entities;

namespace Hauto.Models.DTOs
{
    public class CreateScheduleDto
    {
        public string ScheduleName { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

    public class AddScheduledDeviceDto
    {
        public int ScheduleId { get; set; }
        public int DeviceId { get; set; }
    }
    public class UpdateScheduleDto
    {
        public int Id { get; set; }
        public string ScheduleName { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
    public class UpdateScheduleStatusDto
    {
        public int Id { get; set; }
        public bool OnRepeat { get; set; }
    }
    public class UpdateActiveScheduleStatusDto
    {
        public int Id { get; set; }
        public bool isActive { get; set; }
    }

    public class GetScheduleDto
    {
        public int Id { get; set; }
        public string ScheduleName { get; set; }
        public List<GetScheduleDeviceDto> ScheduleDevice { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool isActive { get; set; }
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
