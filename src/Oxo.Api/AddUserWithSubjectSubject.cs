namespace Oxo.Api;

internal class AddUserWithSubjectSubject
{
    private readonly IUserDataAccess userDataAccess;

    public AddUserWithSubjectSubject(IUserDataAccess userDataAccess)
    {
        this.userDataAccess = userDataAccess;
    }

    public async Task AddUserAsync(User user, string subject)
    {
        await userDataAccess.AddUserAsync(user);
        await userDataAccess.AddSubjectAsync(user.Id, subject);
    }
}
