using E2EChatApp.Core.Application.Services;
using E2EChatApp.Core.Domain.Interfaces;
using E2EChatApp.Core.Helpers;
using E2EChatApp.Core.Domain.BindingModels;
using E2EChatApp.Core.Domain.Models;
using E2EChatApp.Core.Domain.Dtos;
using Microsoft.Extensions.Configuration;
using Moq;
namespace E2EEChatApp.UnitTests.Core.Application.Services {
    public class SecurityServiceTest {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IAuthHelper> _mockAuthHelper;
        private readonly SecurityService _securityService;
        public SecurityServiceTest()
        {
            Mock<IConfiguration> mockConfiguration = new();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockAuthHelper = new Mock<IAuthHelper>();
            _securityService = new SecurityService(mockConfiguration.Object, _mockUserRepository.Object, _mockAuthHelper.Object);
            
            mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("Testing key with a considerable length so the SHA256 hashing algorithm can use it");
            mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("Testing audience");
            mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("Testing issuer");
            mockConfiguration.Setup(config => config["Jwt:ExpirationMinutes"]).Returns("60");
        }

        [Fact]
        public async Task GenerateJwtToken_ShouldReturnTokenDto_WhenUserIsValid()
        {
            // Arrange
            const string email = "test@test.com";
            const string password = "password";
            var user = new UserModel {
                Id = 1,
                Username = "Username",
                PasswordHash = Array.Empty<byte>(),
                PasswordSalt = Array.Empty<byte>()
            };
            _mockUserRepository.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(user);
            _mockAuthHelper.Setup(helper => helper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)).Returns(() => true);
            // Act
            var result = await _securityService.GenerateJwtToken(email, password);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<TokenDto>(result);
        }

        [Fact]
        public async Task GenerateJwtToken_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            string email = "test@test.com";
            string password = "password";
            _mockUserRepository.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(() => null);
            // Act
            var result = await _securityService.GenerateJwtToken(email, password);
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Create_ShouldReturnTrue_WhenNewUserIsCreated()
        {
            // Arrange
            var registerDto = new RegisterDto {
                Email = "test@test.com",
                Password = "password",
                Username = "TestUser",
                PublicKey = "PublicKey"
            };
            _mockUserRepository.Setup(repo => repo.CreateUser(It.IsAny<UserPostBindingModel>())).ReturnsAsync(() => 1);
            // Act
            var result = await _securityService.Create(registerDto.Email, registerDto.Password, registerDto.Username, registerDto.PublicKey);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Create_ShouldReturnFalse_WhenNewUserIsNotCreated()
        {
            // Arrange
            var registerDto = new RegisterDto {
                Email = "test@test.com",
                Password = "password",
                Username = "TestUser",
                PublicKey = "PublicKey"
            };
            _mockUserRepository.Setup(repo => repo.CreateUser(It.IsAny<UserPostBindingModel>())).ReturnsAsync(() => null);
            // Act
            var result = await _securityService.Create(registerDto.Email, registerDto.Password, registerDto.Username, registerDto.PublicKey);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EmailExists_ShouldReturnTrue_WhenEmailExists()
        {
            // Arrange
            string email = "test@test.com";
            var user = new UserModel();
            _mockUserRepository.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(user);
            // Act
            var result = await _securityService.EmailExists(email);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EmailExists_ShouldReturnFalse_WhenEmailDoesNotExists()
        {
            // Arrange
            string email = "test@test.com";
            _mockUserRepository.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync(() => null);
            // Act
            var result = await _securityService.EmailExists(email);
            // Assert
            Assert.False(result);
        }
    }
}