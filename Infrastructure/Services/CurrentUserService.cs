using DomainShared.Constants;
using DomainShared.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor _httpContextAccessor) : ICurrentUser
{
    public string UserId => _httpContextAccessor.HttpContext?.Session.GetString(SessionLogin.UserId) ?? string.Empty;

    public string Email => _httpContextAccessor.HttpContext?.Session.GetString(SessionLogin.Email) ?? string.Empty;
}
