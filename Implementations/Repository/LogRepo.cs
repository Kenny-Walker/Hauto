using Hauto.Context;
using Hauto.Entities;
using Hauto.Interface.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hauto.Implementations.Repository
{
    public class LogRepo : GenericRepo<Log>, ILogRepo
    {
        public LogRepo(HautoContext Context) 
        {
            _Context = Context;
        }
        public async Task<Log> GetLog(int Id)
        {
            return await _Context.Logs.Include(x => x.Device).FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IList<Log>> GetAllLogs()
        {
            return await _Context.Logs.Include(x => x.Device).OrderByDescending(x => x.IsDeleted == false).ToListAsync();
        }
    }
}
