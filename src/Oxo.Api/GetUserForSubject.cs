namespace Oxo.Api;

public class GetUserForSubject
{
    private readonly IUserDataAccess userDataAccess;

    public GetUserForSubject(IUserDataAccess userDataAccess)
    {
        this.userDataAccess = userDataAccess;
    }

    public Task<User?> GetUserAsync(string subject)
    {
        return userDataAccess.GetUserForSubjectAsync(subject);
    }
}
