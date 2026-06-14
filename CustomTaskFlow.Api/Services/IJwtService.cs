using CustomTaskFlow.Api.Models;

namespace CustomTaskFlow.Api.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
