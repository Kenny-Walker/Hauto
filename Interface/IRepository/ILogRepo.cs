using Hauto.Entities;

namespace Hauto.Interface.IRepository
{
    public interface ILogRepo : IGenericRepo<Log>
    {
        Task<Log> GetLog(int Id);
        Task<IList<Log>> GetAllLogs();
    }
}
