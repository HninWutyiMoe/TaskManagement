using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskManagement.DesignTime
{
 public class AppDbContextFactory : IDesignTimeDbContextFactory<DbContext>
 {
 public DbContext CreateDbContext(string[] args)
 {
 var builder = new DbContextOptionsBuilder<DbContext>();
 var conn = "Server=(localdb)\\mssqllocaldb;Database=TaskManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true";
 builder.UseSqlServer(conn);
 return new DbContext(builder.Options);
 }
 }
}
