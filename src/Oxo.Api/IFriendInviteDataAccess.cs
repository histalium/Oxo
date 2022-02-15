namespace Oxo.Api;

public interface IFriendInviteDataAccess
{
    Task AddFriendInvite(FriendInvite friendInvite);
    Task<IReadOnlyList<FriendInvite>> GetFriendInvitesAsync(Guid userId);
}
