using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Foras_Khadra.Models;

namespace Foras_Khadra.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TeamMember> TeamMember { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }


    }
}
