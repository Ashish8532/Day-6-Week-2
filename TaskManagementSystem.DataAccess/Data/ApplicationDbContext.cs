

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models.Authentication;
using TaskManagementSystem.Models.Models;

namespace TaskManagementSystem.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Comments> Comments { get; set; }

    }
}
