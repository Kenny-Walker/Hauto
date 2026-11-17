using Hauto.Models.DTOs;

namespace Hauto.Interface.IService
{
    public interface IScheduleService
    {
        Task<BaseResponse> CreateSchedule(CreateScheduleDto createSchedule);
        Task<BaseResponse> AddScheduledDevice(AddScheduledDeviceDto addDevice);
        Task<BaseResponse> UpdateSchedule(UpdateScheduleDto updateSchedule);
        Task<BaseResponse> UpdateScheduleStatus(UpdateScheduleStatusDto updateScheduleStatus);
        Task<BaseResponse> UpdateActiveScheduleStatus(UpdateActiveScheduleStatusDto updateActiveScheduleStatus);
        Task<BaseResponse> DeleteSchedule(int Id);
        Task<BaseResponse> GetSchedule(int Id);
        Task<ScheduleResponse> GetAllSchedules(int User);
        Task<BaseResponse> UpdateScheduledDevices(UpdateScheduleDeviceStatusDto updateStatus);
        Task<BaseResponse> CheckSchedule(int? ScheduleId, int UserId);
        
    }
}
