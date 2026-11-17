using Hauto.Entities;
using Hauto.Interface.IRepository;
using Hauto.Interface.IService;
using Hauto.Models.DTOs;

namespace Hauto.Implementations.Service
{
    public class UserService : IUserService
    {
        IUserRepo _userRepo;
        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<BaseResponse> AddPasscode(AddPasscodeDto addPasscode)
        {
            var user = await _userRepo.GetUserId(addPasscode.UserId);
            if(user == null)
            {
                return new BaseResponse()
                {
                    Message = "User not found",
                    Success = false
                };
            }
            user.Passcode = addPasscode.Passcode;
            await _userRepo.UpdateAsync(user);
            return new BaseResponse()
            {
                Message = "PassCode Updated",
                Success = true
            };
        }

        public async Task<BaseResponse> CreateUser(CreateUserDto createUser)
        {
            var check = await _userRepo.GetUser(createUser.Email);
            if(check != null)
            {
                return new BaseResponse()
                {
                    Message = "User already exists",
                    Success = false
                };
            }
            var user = new User()
            {
                FirstName = createUser.FirstName,
                LastName = createUser.LastName,
                Email = createUser.Email,
                Password = createUser.Password,
                Passcode = null
            };
            await _userRepo.CreateAsync(user);
            return new BaseResponse()
            {
                Message = "User created successfully",
                Success = true
            };
        }

        public async Task<BaseResponse> GetUser(string email, string password)
        {
            var user = await _userRepo.GetUser(email);
            if(user != null && user.Password == password)
            {
                return new UserResponse
                {
                    Data = GetUserDetails(user),
                    Success = true,
                    Message = "User Profile Retrieved Successfully"
                };
            }
            return new BaseResponse()
            {
                Message = "Cannot Retrieve User Profile",
                Success = false
            };
        }
        public GetUserDto GetUserDetails(User x)
        {
            return new GetUserDto()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Password = x.Password,
                Passcode = x.Passcode
            };
        }

        public async Task<BaseResponse> UpdateUser(UpdateUserDto updateUser)
        {
            var user = await _userRepo.GetUserId(updateUser.UserId);
            if (user == null)
            {
                return new BaseResponse()
                {
                    Message = "User already exists",
                    Success = false
                };
            }
            user.FirstName = updateUser.FirstName;
            user.LastName = updateUser.LastName;
            user.Email = updateUser.Email;
            user.Password = updateUser.Password;
            await _userRepo.UpdateAsync(user);
            return new BaseResponse()
            {
                Message = "User updated successfully",
                Success = true
            };
        }
    }
}
