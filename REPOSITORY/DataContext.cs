using Microsoft.EntityFrameworkCore;
using MODEL.Eneities;

namespace REPOSITORY
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
        
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<TaskHeader> TaskHeader { get; set; }
        public DbSet<TaskDetail> TaskDetail { get; set; }
        public DbSet<UploadedFile> UploadedFile { get; set; }

    }
}
