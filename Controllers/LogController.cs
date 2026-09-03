using Hauto.Interface.IService;
using Microsoft.AspNetCore.Mvc;

namespace Hauto.Controllers
{
    [Route("Hauto/[controller]")]
    [ApiController]
    public class LogController : Controller
    {
        ILogService _logService;
        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("GetLog")]
        public async Task<IActionResult> GetLogById(int logId)
        {
            var log = await _logService.GetLog(logId);
                return Ok(log);
        }

        [HttpGet("GetLogs")]
        public async Task<IActionResult> GetLogs()
        {
            var log = await _logService.GetAllLogs();
            if (log.Success == true)
            {
                return Ok(log);
            }
            return BadRequest(log);
        }
    }
}
