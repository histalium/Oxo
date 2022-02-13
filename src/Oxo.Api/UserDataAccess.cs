namespace Oxo.Api;

using static System.Threading.Tasks.Task;

public class UserDataAccess : IUserDataAccess
{
    private static readonly List<User> _users = new List<User>();
    private static readonly List<(string Subject, Guid UserId)> _subjects = new List<(string Subject, Guid UserId)>();

    public Task<User?> GetUserForSubjectAsync(string subject)
    {
        var haveSubject = _subjects
            .Where(t => t.Subject == subject)
            .Any();

        if (!haveSubject)
        {
            return FromResult<User?>(null);
        }

        var userId = _subjects
            .Where(t => t.Subject == subject)
            .First().UserId;

        var user = _users
            .Where(t => t.Id == userId)
            .First();

        return FromResult<User?>(user);
    }

    public Task AddUserAsync(User user)
    {
        _users.Add(user);

        return CompletedTask;
    }

    public Task AddSubjectAsync(Guid userId, string subject)
    {
        _subjects.Add((subject, userId));

        return CompletedTask;
    }

    public Task<User?> GetUserAsync(Guid userId)
    {
        var user = _users
            .Where(t => t.Id == userId)
            .FirstOrDefault();

        return FromResult(user);
    }
}
