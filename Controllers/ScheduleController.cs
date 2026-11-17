using Hauto.Interface.IService;
using Hauto.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hauto.Controllers
{
    [Route("Hauto/[controller]")]
    [ApiController]
    public class ScheduleController : Controller
    {
        IDeviceService _DeviceService;
        IScheduleService _ScheduleService;
        public ScheduleController(IDeviceService DeviceService, IScheduleService ScheduleService)
        {
            _DeviceService = DeviceService;
            _ScheduleService = ScheduleService;
        }

        [HttpPost("CreateSchedule")]
        public async Task<IActionResult> CreateSchedule([FromForm] CreateScheduleDto createSchedule)
        {
            var Schedule = await _ScheduleService.CreateSchedule(createSchedule); return Ok(Schedule);
        }

        [HttpPost("UpdateSchedule")]
        public async Task<IActionResult> UpdateSchedule([FromForm] UpdateScheduleDto updateSchedule)
        {
            var Schedule = await _ScheduleService.UpdateSchedule(updateSchedule); return Ok(Schedule);
        }

        [HttpPost("UpdateActiveScheduleStatus")]
        public async Task<IActionResult> UpdateActiveScheduleStatus([FromForm] UpdateActiveScheduleStatusDto updateActiveScheduleStatus)
        {
            var Schedule = await _ScheduleService.UpdateActiveScheduleStatus(updateActiveScheduleStatus); return Ok(Schedule);
        }
        
        [HttpPost("UpdateScheduleStatus")]
        public async Task<IActionResult> UpdateScheduleStatus([FromForm] UpdateScheduleStatusDto updateScheduleStatus)
        {
            var Schedule = await _ScheduleService.UpdateScheduleStatus(updateScheduleStatus); return Ok(Schedule);
        }

        [HttpPost("AddScheduledDevice")]
        public async Task<IActionResult> AddScheduledDevice([FromForm] AddScheduledDeviceDto addDevice)
        {
            var Schedule = await _ScheduleService.AddScheduledDevice(addDevice); return Ok(Schedule);
        }

        [HttpPost("UpdateScheduledDevicesStatus")]
        public async Task<IActionResult> UpdateScheduledDevices([FromForm] UpdateScheduleDeviceStatusDto updateStatus)
        {
            var Schedule = await _ScheduleService.UpdateScheduledDevices(updateStatus); return Ok(Schedule);
        }

        [HttpGet("GetSchedule")]
        public async Task<IActionResult> GetSchedule(int Id)
        {
            var Schedule = await _ScheduleService.GetSchedule(Id); return Ok(Schedule);
        }

        [HttpGet("GetAllSchedules")]
        public async Task<IActionResult> GetAllSchedules(int UserId)
        {
            var Schedule = await _ScheduleService.GetAllSchedules(UserId); return Ok(Schedule);
        }

        [HttpDelete("DeleteSchedule")]
        public async Task<IActionResult> DeleteSchedule(int Id)
        {
            var Schedule = await _ScheduleService.DeleteSchedule(Id); return Ok(Schedule);
        }
    }
}
