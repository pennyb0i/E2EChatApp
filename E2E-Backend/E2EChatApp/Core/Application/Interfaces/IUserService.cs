using E2EChatApp.Core.Domain.Models;
namespace E2EChatApp.Core.Application.Interfaces;

public interface IUserService {
    /// <summary>
    /// Get a user by primary key Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<User?> GetUserById(int id);
    Task<List<User>> GetUsers();
    
}
