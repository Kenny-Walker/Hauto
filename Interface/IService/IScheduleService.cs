using Hauto.Models.DTOs;

namespace Hauto.Interface.IService
{
    public interface IScheduleService
    {
        Task<BaseResponse> CreateSchedule(CreateScheduleDto createSchedule);
        Task<BaseResponse> AddScheduledDevice(AddScheduledDeviceDto addDevice);
        Task<BaseResponse> UpdateSchedule(UpdateScheduleDto updateSchedule);
        Task<BaseResponse> UpdateScheduleStatus(UpdateScheduleStatusDto updateScheduleStatus);
        Task<BaseResponse> GetSchedule(int Id);
        Task<ScheduleResponse> GetAllScheduless();
    }
}
