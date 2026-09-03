using Hauto.Entities;
using Hauto.Interface.IRepository;
using Hauto.Interface.IService;
using Hauto.Models.DTOs;

namespace Hauto.Implementations.Service
{
    public class ScheduleService : IScheduleService
    {
        IScheduleRepo _ScheduleRepository;
        public ScheduleService(IScheduleRepo ScheduleRepository)
        {
            _ScheduleRepository = ScheduleRepository;
        }

        public Task<BaseResponse> AddScheduledDevice(AddScheduledDeviceDto addDevice)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> CreateSchedule(CreateScheduleDto createSchedule)
        {
            var Schedule = new Schedule()
            {
                ScheduleName = createSchedule.ScheduleName,
            };
            await _ScheduleRepository.CreateAsync(Schedule);
            return new BaseResponse()
            {
                Message = "Schedule Added Succesfully",
                Success = true
            };
        }

        public async Task<ScheduleResponse> GetAllSchedules()
        {
            var Schedule = await _ScheduleRepository.GetAllSchedules();
            if (Schedule == null)
            {
                return new ScheduleResponse()
                {
                    Data = null,
                    Message = "Couldn't Retrieve Schedules",
                    Success = false
                };
            }
            var SchedulesList = new List<GetScheduleDto>();
            foreach (var x in Schedule) SchedulesList.Add(GetScheduleDetails(x));
            return new ScheduleResponse
            {
                Data = SchedulesList,
                Success = true,
                Message = "Schedules Retrieved Successfully"
            };
        }

        public Task<ScheduleResponse> GetAllScheduless()
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> GetSchedule(int Id)
        {
            var Schedule = await _ScheduleRepository.GetSchedule(Id);
            if (Schedule == null)
            {
                return new SingleScheduleResponse()
                {
                    Data = null,
                    Message = "Schedule does not exist",
                    Success = false
                };
            }
            return new SingleScheduleResponse()
            {
                Data = GetScheduleDetails(Schedule),
                Message = "Schedule Retrieved",
                Success = true
            };
        }

        public GetScheduleDto GetScheduleDetails(Schedule x)
        {
            return new GetScheduleDto()
            {
                Id = x.Id,
                ScheduleName = x.ScheduleName,
                OnRepeat = x.OnRepeat,
                DeviceId = x.DeviceId,
                Device = new GetDeviceDto()
                {
                    Id = x.Device.Id,
                    DeviceName = x.Device.DeviceName,
                    isActive = x.Device.isActive
                }
                
            };
        }

        public async Task<BaseResponse> UpdateSchedule(UpdateScheduleDto updateSchedule)
        {
            var Schedule = await _ScheduleRepository.GetSchedule(updateSchedule.Id);
            if (Schedule == null)
            {
                return new BaseResponse()
                {
                    Message = "Schedule does not exist",
                    Success = false
                };
            }
            Schedule.ScheduleName = updateSchedule.ScheduleName;
            await _ScheduleRepository.UpdateAsync(Schedule);
            return new BaseResponse()
            {
                Message = "Schedule Updated",
                Success = true
            };
        }

        public async Task<BaseResponse> UpdateScheduleStatus(UpdateScheduleStatusDto updateScheduleStatus)
        {
            var Schedule = await _ScheduleRepository.GetSchedule(updateScheduleStatus.Id);
            if (Schedule == null)
            {
                return new BaseResponse()
                {
                    Message = "Schedule does not exist",
                    Success = false
                };
            }
            Schedule.OnRepeat = updateScheduleStatus.OnRepeat;
            await _ScheduleRepository.UpdateAsync(Schedule);
            return new BaseResponse()
            {
                Message = "Schedule Status Updated",
                Success = true
            };
        }
    }
}
