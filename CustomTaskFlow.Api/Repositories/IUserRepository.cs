using CustomTaskFlow.Api.Models;

namespace CustomTaskFlow.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUserNameAsync(string userName);
        Task<User> CreateAsync(User user);
    }
}
