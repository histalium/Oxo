namespace Oxo.Api;

public interface IUserDataAccess
{
    Task AddSubjectAsync(Guid userId, string subject);
    Task AddUserAsync(User user);
    Task<User?> GetUserForSubjectAsync(string subject);
    Task<User?> GetUserAsync(Guid userId);
}
