using E2EChatApp.Core.Entities;
namespace E2EChatApp.Core.Services.Interfaces;

public interface IUserService {
    /// <summary>
    /// Get a user by primary key Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<User> GetUserById(int id);
}
