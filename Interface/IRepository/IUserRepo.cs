using Hauto.Entities;

namespace Hauto.Interface.IRepository
{
    public interface IUserRepo : IGenericRepo<User>
    {
        Task<User> GetUser(string email);
        Task<User> GetUserId(int Id);
    }
}
