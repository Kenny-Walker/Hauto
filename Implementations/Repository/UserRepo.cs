using Hauto.Context;
using Hauto.Entities;
using Hauto.Interface.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hauto.Implementations.Repository
{
    public class UserRepo : GenericRepo<User>, IUserRepo
    {
        public UserRepo(HautoContext Context)
        {
            _Context = Context;
        }

        public async Task<User> GetUser(string email)
        {
            return await _Context.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsDeleted == false);
        }
        public async Task<User> GetUserId(int Id)
        {
            return await _Context.Users.FirstOrDefaultAsync(x => x.Id == Id && x.IsDeleted == false);
        }
    }
}
