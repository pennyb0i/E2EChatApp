using E2EChatApp.Core.Entities;
using E2EChatApp.Core.Services.Interfaces;
namespace E2EChatApp.Core.Services;

public class UserService : IUserService{

    public UserService()
    {
        
    }

    public async Task<User> GetUserById(int id)
    {
        throw new NotImplementedException();
    }
}
