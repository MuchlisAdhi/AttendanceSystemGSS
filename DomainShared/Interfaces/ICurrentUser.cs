namespace DomainShared.Interfaces;

public interface ICurrentUser
{
    string UserId { get; }
    string Email { get; }
}
