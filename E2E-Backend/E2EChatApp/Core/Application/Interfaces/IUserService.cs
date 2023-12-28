using E2EChatApp.Core.Domain.Dtos;
using E2EChatApp.Core.Domain.Models;
namespace E2EChatApp.Core.Application.Interfaces;

public interface IUserService {

    /// <summary>
    /// Get all users in the system
    /// </summary>
    /// <param name="friendsOnly"></param>
    /// <param name="currentUserId"></param>
    /// <returns></returns>
    public Task<List<UserDto>> GetAllUsers(bool? friendsOnly, int currentUserId);
    
    /// <summary>
    /// Get a user by primary key Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<UserModel?> GetUserById(int id);
    Task<List<UserModel>> GetUsers();
    
}
