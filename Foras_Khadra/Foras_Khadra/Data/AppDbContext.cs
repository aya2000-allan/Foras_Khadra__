using Microsoft.EntityFrameworkCore;

namespace Foras_Khadra.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

      
    }
}
