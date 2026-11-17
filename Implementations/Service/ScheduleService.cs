using Hauto.Entities;
using Hauto.Interface.IRepository;
using Hauto.Interface.IService;
using Hauto.Models.DTOs;

namespace Hauto.Implementations.Service
{
    public class ScheduleService : IScheduleService
    {
        IScheduleRepo _ScheduleRepository;
        IScheduledDeviceRepo _SchedulededDeviceRepository;
        IDeviceRepo _deviceRepository;
        public ScheduleService(IScheduleRepo ScheduleRepository, IScheduledDeviceRepo SchedulededDeviceRepository, IDeviceRepo deviceRepository)
        {
            _ScheduleRepository = ScheduleRepository;
            _SchedulededDeviceRepository = SchedulededDeviceRepository;
            _deviceRepository = deviceRepository;
        }

        public async Task<BaseResponse> AddScheduledDevice(AddScheduledDeviceDto addDevice)
        {
            var schedule = await _ScheduleRepository.GetSchedule(addDevice.ScheduleId);
            var device = await _deviceRepository.GetDevice(addDevice.DeviceId);
            if (schedule == null || device == null) 
            {
                return new BaseResponse()
                {
                    Message = "Schedule not found",
                    Success = false
                };

            }
            else if(schedule != null && device != null)
            {
                var existingScheduledDevice = await _SchedulededDeviceRepository.GetAsync(x => x.DeviceId == addDevice.DeviceId && x.ScheduleId == addDevice.ScheduleId);
                if (existingScheduledDevice != null)
                {
                    return new BaseResponse()
                    {
                        Message = "Device already scheduled",
                        Success = false
                    };
                }
                var ScheduleDevice = new ScheduledDevice()
                {
                    DeviceId = addDevice.DeviceId,
                    ScheduleId = addDevice.ScheduleId,
                
                };
                await _SchedulededDeviceRepository.CreateAsync(ScheduleDevice);
                return new BaseResponse()
                {
                    Message = "Device Added to Schedule Succesfully",
                    Success = true
                };
            }
            return new BaseResponse()
            {
                Message = "Unable to add Device to Schedule Succesfully",
                Success = false
            };
        }

        public async Task<BaseResponse> CreateSchedule(CreateScheduleDto createSchedule)
        {
            var Schedule = new Schedule()
            {
                UserId = createSchedule.UserId,
                ScheduleName = createSchedule.ScheduleName,
                StartTime = createSchedule.StartTime,
                EndTime = createSchedule.EndTime,
            };
            await _ScheduleRepository.CreateAsync(Schedule);
            return new BaseResponse()
            {
                Message = "Schedule Added Succesfully",
                Success = true
            };
        }

        public async Task<BaseResponse> DeleteSchedule(int Id)
        {
            var schedule = await _ScheduleRepository.GetSchedule(Id);
            if (schedule == null)
            {
                return new BaseResponse()
                {
                    Message = "Schedule not found",
                    Success = false
                };
            }
            schedule.IsDeleted = true;
            await _ScheduleRepository.UpdateAsync(schedule);
            return new BaseResponse()
            {
                Message = "Schedule Deleted Successfully",
                Success = true
            };
        }

        public async Task<ScheduleResponse> GetAllSchedules(int UserId)
        {
            var Schedule = await _ScheduleRepository.GetAllSchedules(UserId);
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

        public async Task<ScheduleDeviceResponse> GetAllDevicesByScheduleId(int ScheduleId)
        {
            var Schedule = await _SchedulededDeviceRepository.GetScheduledDevices(ScheduleId);
            if (Schedule == null)
            {
                return new ScheduleDeviceResponse()
                {
                    Data = null,
                    Message = "Couldn't Retrieve Schedule Devices",
                    Success = false
                };
            }
            var ScheduleDeviceList = new List<GetScheduleDeviceDto>();
            foreach (var x in Schedule) ScheduleDeviceList.Add(GetScheduleDeviceDetails(x));
            return new ScheduleDeviceResponse
            {
                Data = ScheduleDeviceList,
                Success = true,
                Message = "Schedule Devices Retrieved Successfully"
            };
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

        public GetScheduleDto GetScheduleDetails(Schedule c)
        {
            return new GetScheduleDto()
            {
                Id = c.Id,
                UserId = c.UserId,
                ScheduleName = c.ScheduleName,
                OnRepeat = c.OnRepeat,
                isActive = c.isActive,
                StartTime = c.StartTime,
                EndTime = c.EndTime,
                ScheduleDevice = c.ScheduledDevice.Select(x => new GetScheduleDeviceDto()
                {
                    Id = x.Id,
                    DeviceId = x.DeviceId,
                    isActive = x.isActive,
                    Device = new GetDeviceDto()
                    {
                        Id = x.Device.Id,
                        UserId = x.Device.UserId,
                        HubId = x.Device.HubId,
                        Hub = new GetHubsDto()
                        {
                            Id = x.Device.Hub.Id,
                            UserId = x.Device.Hub.UserId,
                            HubName = x.Device.Hub.HubName,
                        },
                        DeviceName = x.Device.DeviceName,
                        isActive = x.Device.isActive,
                    },
                    ScheduleId = x.ScheduleId,
                }).ToList(),
            };
        }
        public GetScheduleDeviceDto GetScheduleDeviceDetails(ScheduledDevice x)
        {
            return new GetScheduleDeviceDto()
            {
                Id = x.Id,
                DeviceId = x.DeviceId,
                isActive = x.isActive,
                Device = new GetDeviceDto()
                {
                    Id = x.Device.Id,
                    UserId = x.Device.UserId,
                    HubId = x.Device.HubId,
                    DeviceName = x.Device.DeviceName,
                    isActive = x.Device.isActive,
                },
                ScheduleId = x.ScheduleId,
                Schedule = new GetScheduleDto()
                {
                    Id = x.Schedule.Id,
                    UserId = x.Schedule.UserId,
                    ScheduleName = x.Schedule.ScheduleName,
                    OnRepeat = x.Schedule.OnRepeat,
                    isActive = x.Schedule.isActive,
                },
            };
        }

        public async Task<BaseResponse> UpdateActiveScheduleStatus(UpdateActiveScheduleStatusDto updateActiveScheduleStatus)
        {
            var Schedule = await _ScheduleRepository.GetSchedule(updateActiveScheduleStatus.Id);
            if (Schedule == null)
            {
                return new BaseResponse()
                {
                    Message = "Schedule does not exist",
                    Success = false
                };
            }
            Schedule.isActive = updateActiveScheduleStatus.isActive;
            await _ScheduleRepository.UpdateAsync(Schedule);
            return new BaseResponse()
            {
                Message = "Schedule Status Updated",
                Success = true
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
            Schedule.StartTime = updateSchedule.StartTime;
            Schedule.EndTime = updateSchedule.EndTime;
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

        public async Task<BaseResponse> UpdateScheduledDevices(UpdateScheduleDeviceStatusDto updateStatus)
        {
            var device = await _deviceRepository.GetDevice(updateStatus.DeviceId);
            var scheduledDevice = await _SchedulededDeviceRepository.GetScheduledDevice(updateStatus.DeviceId);
            if(device == null || scheduledDevice == null)
            {
                return new BaseResponse()
                {
                    Message = "Can't Retrieve Device",
                    Success = false
                };
            }
            scheduledDevice.isActive = updateStatus.isActive;
            device.isActive = updateStatus.isActive;
            await _SchedulededDeviceRepository.UpdateAsync(scheduledDevice);
            await _deviceRepository.UpdateAsync(device);
            return new BaseResponse()
            {
                Message = "Device Status Updated",
                Success = true,
            };
        }

        public async Task<BaseResponse> CheckSchedule(int? ScheduleId, int UserId)
        {
            
            if (ScheduleId == null)
            {
                var schedules = await _ScheduleRepository.GetAllSchedules(UserId);
                var scheduledDevice = await _SchedulededDeviceRepository.GetScheduledDevices(ScheduleId ?? 0);
                foreach (var Schedule in schedules)
                {
                    if (Schedule != null && Schedule.StartTime <= TimeOnly.FromDateTime(DateTime.Now) && Schedule.EndTime >= TimeOnly.FromDateTime(DateTime.Now))
                    {
                        if (Schedule.OnRepeat == true)
                        {
                            Schedule.isActive = true;
                            await _ScheduleRepository.UpdateAsync(Schedule);

                            if (scheduledDevice == null || scheduledDevice.Count == 0)
                            {
                                return new BaseResponse()
                                {
                                    Message = "No devices scheduled for this schedule",
                                    Success = false
                                };
                            }
                            foreach (var device in scheduledDevice)
                            {
                                var checkDevice = await _deviceRepository.GetDevice(device.DeviceId);

                                checkDevice.isActive = device.isActive;
                                await _SchedulededDeviceRepository.UpdateAsync(device);
                                var deviceEntity = await _deviceRepository.GetDevice(device.DeviceId);
                                if (deviceEntity != null)
                                {
                                    deviceEntity.isActive = device.isActive;
                                    await _deviceRepository.UpdateAsync(deviceEntity);
                                }
                            }
                            return new BaseResponse()
                            {
                                Message = "Scheduled devices status toggled on",
                                Success = true
                            };
                        }
                        return new BaseResponse()
                        {
                            Message = "Scheduled devices status toggled on",
                            Success = true
                        };
                    }
                }

            }
            var schedule = await _ScheduleRepository.GetSchedule(ScheduleId ?? 0);
            var scheduledDevices = await _SchedulededDeviceRepository.GetScheduledDevices(ScheduleId ?? 0);
            if (schedule != null && schedule.StartTime <= TimeOnly.FromDateTime(DateTime.Now) && schedule.EndTime >= TimeOnly.FromDateTime(DateTime.Now))
            {
                if(schedule.OnRepeat == true)
                {
                    schedule.isActive = true;
                    await _ScheduleRepository.UpdateAsync(schedule);

                    if (scheduledDevices == null || scheduledDevices.Count == 0)
                    {
                        return new BaseResponse()
                        {
                            Message = "No devices scheduled for this schedule",
                            Success = false
                        };
                    }
                    foreach (var device in scheduledDevices)
                    {
                        var checkDevice = await _deviceRepository.GetDevice(device.DeviceId);

                        checkDevice.isActive = device.isActive;
                        await _SchedulededDeviceRepository.UpdateAsync(device);
                        var deviceEntity = await _deviceRepository.GetDevice(device.DeviceId);
                        if (deviceEntity != null)
                        {
                            deviceEntity.isActive = device.isActive;
                            await _deviceRepository.UpdateAsync(deviceEntity);
                        }
                    }
                    return new BaseResponse()
                    {
                        Message = "Scheduled devices status toggled on",
                        Success = true
                    };
                }
                return new BaseResponse()
                {
                    Message = "Scheduled devices status toggled on",
                    Success = true
                };
            }
            if (schedule != null  && schedule.EndTime <= TimeOnly.FromDateTime(DateTime.Now))
            {
                schedule.isActive = false;
                await _ScheduleRepository.UpdateAsync(schedule);

                if (scheduledDevices == null || scheduledDevices.Count == 0)
                {
                    return new BaseResponse()
                    {
                        Message = "No devices scheduled for this schedule",
                        Success = false
                    };
                }
                foreach (var device in scheduledDevices)
                {

                    var deviceEntity = await _deviceRepository.GetDevice(device.DeviceId);
                    if (deviceEntity != null)
                    {
                        deviceEntity.isActive = false;
                        await _deviceRepository.UpdateAsync(deviceEntity);
                        device.isActive = deviceEntity.isActive;
                        await _SchedulededDeviceRepository.UpdateAsync(device);
                    }
                }
                return new BaseResponse()
                {
                    Message = "Scheduled devices status toggled off",
                    Success = true
                };
            }
            return new BaseResponse()
            {
                Message = "Schedule is not active",
                Success = false
            };
        }
    }
}
