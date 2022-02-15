namespace Oxo.Api;
internal class AddFriendInvite
{
    private readonly IFriendInviteDataAccess friendInviteDataAccess;

    public AddFriendInvite(IFriendInviteDataAccess friendInviteDataAccess)
    {
        this.friendInviteDataAccess = friendInviteDataAccess;
    }

    public Task AddInviteAsync(Guid inviteeId, Guid inviterId)
    {
        return friendInviteDataAccess.AddFriendInvite(new FriendInvite(Guid.NewGuid(), inviteeId, inviterId));
    }
}
