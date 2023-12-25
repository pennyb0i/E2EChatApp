using E2EChatApp.Core.Domain.Dtos;
namespace E2EChatApp.Core.Application.Interfaces;

public interface ISecurityService
{
    Task<TokenDto?> GenerateJwtToken(string email, string password);
    Task<bool> Create(string loginDtoEmail, string loginDtoPassword,string loginDtoUsername,string loginDtoPublicKey);
    Task<bool> EmailExists(string loginDtoEmail);
}
