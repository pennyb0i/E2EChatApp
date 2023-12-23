using System.Net;
using E2EChatApp.Core.Application.Interfaces;
using E2EChatApp.Core.Domain.Dtos;
using E2EChatApp.Core.Domain.Entities;
using E2EChatApp.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace E2EChatApp.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ISecurityService _securityService;

    public AuthController(ISecurityService securityService)
    {
        _securityService = securityService;
    }
    
    [AllowAnonymous] 
    [HttpPost(nameof(Login))]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto loginDto)
    {
        var token = await _securityService.GenerateJwtToken(loginDto.Email, loginDto.Password);
        if (token is null) {
            throw new RestException(HttpStatusCode.BadRequest, "Invalid username or password!");
        }
        return new TokenDto
        {
            Jwt = token.Jwt,
            Message = token.Message,
            UserId = token.UserId,
        };
    }
    
    [AllowAnonymous]
    [HttpPost(nameof(Register))]
    public async Task<ActionResult<TokenDto>> Register([FromBody] LoginDto loginDto)
    {
        var exists = await _securityService.EmailExists(loginDto.Email);
        if(exists)
            throw new RestException(HttpStatusCode.BadRequest, "Email is already in use!");
        if (await _securityService.Create(loginDto.Email, loginDto.Password))
        {
            var token = await _securityService.GenerateJwtToken(loginDto.Email, loginDto.Password);
            if (token is null) {
                throw new RestException(HttpStatusCode.BadRequest, "Something went wrong during the registration process. Please contact an admin");
            }
            return new TokenDto
            {
                Jwt = token.Jwt,
                Message = token.Message,
                UserId = token.UserId,
            };
        }
        throw new RestException(HttpStatusCode.BadRequest, "Something went wrong during the registration process. Please contact an admin");
    }

}
