using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.Models;

namespace CustomTaskFlow.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUserNameAsync(string userName);
        Task<User> CreateAsync(User user);
        Task<PagedResult<User>> GetAllAsync(int pageNumber, int pageSize, string? search);
    }
}
