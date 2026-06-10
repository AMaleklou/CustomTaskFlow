using CustomTaskFlow.Api.Data;
using CustomTaskFlow.Api.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<User?> GetByEmailAsync(string email)
        {

            return await _appDbContext.Users
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(x => x.Email == email);    
        }

        public async Task<User?> GetByUserNameAsync(string userName)
        {
            return await _appDbContext.Users
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(x => x.UserName == userName);
        }
    }
}
