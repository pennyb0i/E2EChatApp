using System.IdentityModel.Tokens.Jwt;
using System.Text;
using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Domain.BindingModels;
using E2EChatApp.Core.Domain.Dtos;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Helpers;
using Microsoft.IdentityModel.Tokens;
namespace E2EChatApp.Core.Application.Services;

public class SecurityService: ISecurityService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthHelper _authenticationHelper;
    
    public SecurityService(IConfiguration configuration, 
        IUserRepository userRepository,  IAuthHelper authenticationHelper)
    {
        Configuration = configuration;
        _userRepository = userRepository;
        _authenticationHelper = authenticationHelper;
    }

    private IConfiguration Configuration { get; }
    
    public async Task<TokenDto?> GenerateJwtToken(string email, string password)
    {
        var user = await _userRepository.GetUserByEmail(email);
        if (user is null)
            return null;
        
        //Did we not find a user with the given username?
        if (_authenticationHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                Configuration["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
                Configuration["Jwt:Audience"],
                null,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            return new TokenDto
            {
                Jwt = new JwtSecurityTokenHandler().WriteToken(token),
                Message = "ok",
                UserId = user.Id
            };
        }

        return null;
    }

    public async Task<bool> Create(string loginDtoEmail, string loginDtoPassword)
    {
        _authenticationHelper.CreatePasswordHash(loginDtoPassword,
            out var hash, out var salt);
        return await _userRepository.CreateUser(new UserPostBindingModel {
            Email = loginDtoEmail,
            PasswordHash = hash,
            PasswordSalt = salt
        }) is not null;
    }

    public async Task<bool> EmailExists(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);
        return user is not null;
    }
}