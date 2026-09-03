using Hauto.Interface.IService;
using Hauto.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hauto.Controllers
{
    [Route("Hauto/[controller]")]
    [ApiController]
    public class DeviceController : Controller
    {
        IDeviceService _DeviceService;
        public DeviceController(IDeviceService DeviceService)
        {
            _DeviceService = DeviceService;
        }

        [HttpPost("AddDevice")]
        public async Task<IActionResult> AddDeviceById([FromForm] CreateDeviceDto createDevice)
        {
            var Device = await _DeviceService.CreateDevice(createDevice); return Ok(Device);
        }

        [HttpPost("UpdateDevice")]
        public async Task<IActionResult> UpdateDevice([FromForm] UpdateDeviceDto updateDevice)
        {
            var Device = await _DeviceService.UpdateDevice(updateDevice); return Ok(Device);
        }

        [HttpPost("ON/OFFDevice")]
        public async Task<IActionResult> OnDevice([FromForm] UpdateDeviceStatusDto updateDeviceStatus)
        {
            var Device = await _DeviceService.UpdateDeviceStatus(updateDeviceStatus); return Ok(Device);
        }

        [HttpGet("GetDevice")]
        public async Task<IActionResult> GetDeviceById(int DeviceId)
        {
            var Device = await _DeviceService.GetDevice(DeviceId); return Ok(Device);
        }

        [HttpGet("GetDevices")]
        public async Task<IActionResult> GetDevices()
        {
            var Device = await _DeviceService.GetAllDevices(); return Ok(Device);
        }
    }
}
