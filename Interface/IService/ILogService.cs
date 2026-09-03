using Hauto.Models.DTOs;

namespace Hauto.Interface.IService
{
    public interface ILogService
    {
        Task<BaseResponse> GetLog(int Id);
        Task<LogResponse> GetAllLogs();
    }
}
