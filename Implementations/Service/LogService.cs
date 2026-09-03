using Hauto.Entities;
using Hauto.Interface.IRepository;
using Hauto.Interface.IService;
using Hauto.Models.DTOs;

namespace Hauto.Implementations.Service
{
    public class LogService : ILogService
    {
        ILogRepo _LogRepository;
        public LogService(ILogRepo LogRepository)
        {
            _LogRepository = LogRepository;
        }
        public async Task<LogResponse> GetAllLogs()
        {
            var Log = await _LogRepository.GetAllLogs();
            if (Log == null)
            {
                return new LogResponse()
                {
                    Data = null,
                    Message = "Couldn't Retrieve Logs",
                    Success = false
                };
            }
            var LogsList = new List<GetLogDto>();
            foreach (var x in Log) LogsList.Add(GetLogDetails(x));
            return new LogResponse
            {
                Data = LogsList,
                Success = true,
                Message = "Logs Retrieved Successfully"
            };
        }

        public async Task<BaseResponse> GetLog(int Id)
        {
            var Log = await _LogRepository.GetLog(Id);
            if (Log == null)
            {
                return new SingleLogResponse()
                {
                    Data = null,
                    Message = "Log does not exist",
                    Success = false
                };
            }
            return new SingleLogResponse()
            {
                Data = GetLogDetails(Log),
                Message = "Log Retrieved",
                Success = true
            };
        }

        public GetLogDto GetLogDetails(Log x)
        {
            return new GetLogDto()
            {
                Id = x.Id,
                LogStatement = x.LogStatement,
                Date = x.Date,
                DeviceId = x.DeviceId,
                Device = new GetDeviceDto()
                {
                    Id = x.Device.Id,
                    DeviceName = x.Device.DeviceName,
                    isActive = x.Device.isActive
                }
            };
        }

    }
}
