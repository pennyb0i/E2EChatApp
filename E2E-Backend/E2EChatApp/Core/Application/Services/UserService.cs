using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Domain.Models;
namespace E2EChatApp.Core.Application.Services;

public class UserService : IUserService{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserById(int id)
    {
        return await _userRepository.GetUserById(id);
    }

    public async Task<List<User>> GetUsers()
    {
        return await _userRepository.GetUsers();
    }
}
