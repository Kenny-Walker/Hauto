using Hauto.Entities;

namespace Hauto.Interface.IRepository
{
    public interface IHubsRepo : IGenericRepo<Hubs>
    {
        Task<Hubs> GetHub(int Id);
        Task<IList<Hubs>> GetAllUserHubs(int UserId);
    }
}
