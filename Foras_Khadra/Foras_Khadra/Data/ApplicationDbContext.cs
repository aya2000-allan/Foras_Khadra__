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
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<ReelsRequest> ReelsRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Organization>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReelsRequest>()
                .HasOne(r => r.Opportunity)
                .WithMany()
                .HasForeignKey(r => r.OpportunityId)
                .OnDelete(DeleteBehavior.Cascade); // OK

            modelBuilder.Entity<ReelsRequest>()
                .HasOne(r => r.Organization)
                .WithMany()
                .HasForeignKey(r => r.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict); // ← منع الـ cascade هنا
        }


    }
}
