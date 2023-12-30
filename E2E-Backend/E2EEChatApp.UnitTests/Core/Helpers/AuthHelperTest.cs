using E2EChatApp.Core.Helpers;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Moq;

namespace E2EEChatApp.UnitTests.Core.Helpers;

[TestSubject(typeof(AuthHelper))]
public class AuthHelperTest {

    private readonly AuthHelper _authHelper;
    private readonly Mock<IConfiguration> _mockConfiguration = new();

    public AuthHelperTest()
    {
        _authHelper = new AuthHelper(_mockConfiguration.Object);
        _mockConfiguration.Setup(config => config["Jwt:Pepper"]).Returns("TestPepper");
    }

    [Fact]
    public void CreatePasswordHash_GivenPassword_OutputsHashAndSalt()
    {
        const string password = "TestPassword";

        _authHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

        Assert.NotNull(passwordHash);
        Assert.NotNull(passwordSalt);
    }

    [Fact]
    public void VerifyPasswordHash_GivenValidInputs_ReturnsTrue()
    {
        const string password = "TestPassword";

        _authHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        var verifyResult = _authHelper.VerifyPasswordHash(password, passwordHash, passwordSalt);

        Assert.True(verifyResult);
    }

    [Fact]
    public void VerifyPasswordHash_GivenInvalidInputs_ReturnsFalse()
    {
        const string password = "TestPassword";
        const string wrongPassword = "WrongPassword";

        _authHelper.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        var verifyResult = _authHelper.VerifyPasswordHash(wrongPassword, passwordHash, passwordSalt);

        Assert.False(verifyResult);
    }
}