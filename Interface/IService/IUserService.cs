using Hauto.Interface.IRepository;
using Hauto.Models.DTOs;

namespace Hauto.Interface.IService
{
    public interface IUserService
    {
        Task<BaseResponse> GetUser(string email, string password);
        Task<BaseResponse> CreateUser(CreateUserDto createUser);
        Task<BaseResponse> UpdateUser(UpdateUserDto updateUser);
        Task<BaseResponse> AddPasscode(AddPasscodeDto addPasscode);
    }
}
