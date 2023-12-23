using E2EChatApp.Core.Domain.Dtos;
namespace E2EChatApp.Core.Application.Interfaces;

public interface ISecurityService
{
    Task<TokenDto?> GenerateJwtToken(string email, string password);
    Task<bool> Create(string loginDtoEmail, string loginDtoPassword);
    Task<bool> EmailExists(string loginDtoEmail);
}
