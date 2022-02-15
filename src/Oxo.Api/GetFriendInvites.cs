namespace Oxo.Api;

internal class GetFriendInvites
{
    private readonly IFriendInviteDataAccess friendInviteDataAccess;

    public GetFriendInvites(IFriendInviteDataAccess friendInviteDataAccess)
    {
        this.friendInviteDataAccess = friendInviteDataAccess;
    }

    public Task<IReadOnlyList<FriendInvite>> GetInvitesAsync(Guid userId)
    {
        return this.friendInviteDataAccess.GetFriendInvitesAsync(userId);
    }
}
