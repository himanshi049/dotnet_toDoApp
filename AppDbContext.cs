using Microsoft.EntityFrameworkCore;
using TaskApi.Models;

namespace TaskApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        //Dbset for tasks
        public DbSet<TaskItem> Tasks { get; set; }

        //Dbset for users
        public DbSet<User> Users { get; set; }
    }
}
