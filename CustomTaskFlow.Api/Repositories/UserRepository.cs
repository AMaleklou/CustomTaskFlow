using CustomTaskFlow.Api.Common;
using CustomTaskFlow.Api.Data;
using CustomTaskFlow.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System.Diagnostics;
using CustomTaskFlow.Api.Enums;

namespace CustomTaskFlow.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;
        public UserRepository(AppDbContext appDbContext) 
        {
           _appDbContext = appDbContext;
        }

        public async Task<User> CreateAsync(User user)
        {
             await _appDbContext.Users.AddAsync(user);
             await _appDbContext.SaveChangesAsync();
             return user;
        }

        public async Task<int> GetAdminCountAsync()
        {
            return await _appDbContext.Users
                                      .AsNoTracking()
                                       .CountAsync(u => u.Role == UserRole.Admin);                                           
        }

        public async Task<PagedResult<User>> GetAllAsync(int pageNumber, int pageSize, string? search)
        {
            var userQuery = _appDbContext.Users.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                userQuery = userQuery.Where(u => u.Email.Contains(search) || (u.UserName != null && u.UserName.Contains(search)));
            }
            var totalCount = await userQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            userQuery = userQuery.OrderByDescending(t => t.CreatedAt);
            userQuery = userQuery.Skip((pageNumber - 1) * pageSize);
            userQuery = userQuery.Take(pageSize);

            var userList = await userQuery.ToListAsync();

            var result = new PagedResult<User>
            {
                Items = userList,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return  result;

        }

        public async Task<User?> GetByEmailAsync(string email)
        {

            return await _appDbContext.Users
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(x => x.Email == email);    
        }

        public async Task<User?> GetByIdForUpdateAsync(int id)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _appDbContext.Users
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
