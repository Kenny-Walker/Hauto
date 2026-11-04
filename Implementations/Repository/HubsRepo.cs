using Hauto.Context;
using Hauto.Entities;
using Hauto.Interface.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hauto.Implementations.Repository
{
    public class HubsRepo : GenericRepo<Hubs>, IHubsRepo
    {
        public HubsRepo(HautoContext Context)
        {
            _Context = Context;
        }

        public async Task<IList<Hubs>> GetAllHubs()
        {
            return await _Context.Hubs.Include(x => x.Devices).OrderByDescending(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<Hubs> GetHub(int Id)
        {
            return await _Context.Hubs.Include(x => x.Devices).FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == Id);
        }
    }
}
