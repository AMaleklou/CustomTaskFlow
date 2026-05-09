using CustomTaskFlow.Api.Models;
using Microsoft.EntityFrameworkCore;


namespace CustomTaskFlow.Api.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<TaskItem> Tasks { get; set; }

    }
}
