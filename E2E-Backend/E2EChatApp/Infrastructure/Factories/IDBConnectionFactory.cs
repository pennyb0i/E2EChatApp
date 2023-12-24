using System.Data;
namespace E2EChatApp.Infrastructure.Factories;

/// <summary>
/// Service capable of creating database connections on demand.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Creates a database connection and returns it ready for use (in an open state).
    /// </summary>
    /// <returns>The newly created database connection.</returns>
    Task<IDbConnection> CreateAsync();
}
