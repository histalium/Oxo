namespace Oxo.Api;

using static System.Threading.Tasks.Task;

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

public interface IFriendInviteDataAccess
{
    Task AddFriendInvite(FriendInvite friendInvite);
}

public class FriendInviteDataAccess : IFriendInviteDataAccess
{
    private static List<FriendInvite> _invites = new List<FriendInvite>();

    public Task AddFriendInvite(FriendInvite friendInvite)
    {
        _invites.Add(friendInvite);
        return CompletedTask;
    }
}

public record FriendInvite(
    Guid Id,
    Guid InviteeId,
    Guid InviterId
);