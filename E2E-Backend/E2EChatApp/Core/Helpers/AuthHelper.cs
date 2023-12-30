using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
namespace E2EChatApp.Core.Helpers;

public class AuthHelper: IAuthHelper
{
    private IConfiguration Configuration { get; }
    public AuthHelper(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        var pepper = Configuration["Jwt:Pepper"] ?? throw new NullReferenceException("Pepper cannot be null");
        // Generate salt (16 bytes)
        passwordSalt = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(passwordSalt);
        }

        // Include the pepper
        string passwordWithPepper = password + pepper;

        // Argon2id hasher
        using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(passwordWithPepper)))
        {
            argon2.Salt = passwordSalt;

            // Hashing
            argon2.DegreeOfParallelism = 8;
            argon2.Iterations = 4;
            argon2.MemorySize = 65536;
            passwordHash = argon2.GetBytes(64);
        }
    }

    public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        var pepper = Configuration["Jwt:Pepper"] ?? throw new NullReferenceException("Pepper cannot be null");
        // Include the pepper
        var passwordWithPepper = password + pepper;

        // Create an Argon2id verifier
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(passwordWithPepper));
        argon2.Salt = storedSalt;
        argon2.DegreeOfParallelism = 8;
        argon2.Iterations = 4;
        argon2.MemorySize = 65536;

        var inputPasswordHash = argon2.GetBytes(64);

        return storedHash.SequenceEqual(inputPasswordHash);
    }
}
