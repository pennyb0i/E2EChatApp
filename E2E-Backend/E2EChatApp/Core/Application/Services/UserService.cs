using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Domain.Dtos;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;
namespace E2EChatApp.Core.Application.Services;

public class UserService : IUserService{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserDto>> GetAllUsers(bool? friendsOnly, int currentUserId)
    {
        return await _userRepository.GetAllUsers(friendsOnly, currentUserId);
    }

    public async Task<UserModel?> GetUserById(int id)
    {
        return await _userRepository.GetUserById(id);
    }

    public async Task<List<User>> GetUsers()
    {
        return await _userRepository.GetUsers();
    }
}
