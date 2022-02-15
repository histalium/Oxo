namespace Oxo.Api;

public record FriendInvite(
    Guid Id,
    Guid InviteeId,
    Guid InviterId
);
