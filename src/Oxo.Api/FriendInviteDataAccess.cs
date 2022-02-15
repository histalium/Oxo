namespace Oxo.Api;

using static System.Threading.Tasks.Task;

public class FriendInviteDataAccess : IFriendInviteDataAccess
{
    private static List<FriendInvite> _invites = new List<FriendInvite>();

    public Task AddFriendInvite(FriendInvite friendInvite)
    {
        _invites.Add(friendInvite);
        return CompletedTask;
    }

    public Task<IReadOnlyList<FriendInvite>> GetFriendInvitesAsync(Guid userId)
    {
        var invites = _invites
            .Where(t => t.InviteeId == userId || t.InviterId == userId)
            .ToList();
        return FromResult<IReadOnlyList<FriendInvite>>(invites);
    }
}
