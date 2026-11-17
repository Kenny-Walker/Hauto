using Hauto.Implementations.Service;
using Hauto.Interface.IService;
using Hauto.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Hauto.Controllers
{
    [Route("Hauto/[controller]")]
    [ApiController]
    public class HubController : Controller
    {
        IHubsService _hubService;
        public HubController(IHubsService hubService)
        {
            _hubService = hubService;
        }

        [HttpPost("CreateHub")]
        public async Task<IActionResult> AddHub([FromForm] CreateHubsDto createHub)
        {
            var hub = await _hubService.CreateHub(createHub); return Ok(hub);
        }

        [HttpPost("UpdateHub")]
        public async Task<IActionResult> UpdateDevice([FromForm] UpdateHubsDto updateHub)
        {
            var hub = await _hubService.UpdateHub(updateHub); return Ok(hub);
        }

        [HttpGet("GetHub")]
        public async Task<IActionResult> GetHubById(int HubId)
        {
            var hub = await _hubService.GetHub(HubId); return Ok(hub);
        }

        [HttpGet("GetHubs")]
        public async Task<IActionResult> GetHubs(int UserId)
        {
            var hub = await _hubService.GetAllHubs(UserId); return Ok(hub);
        }
    }
}
