using CodeLeapChallengeAPI_06022025.Data.Class;
using Microsoft.EntityFrameworkCore;

namespace CodeLeapChallengeAPI_06022025.Data.Context
{
    public class CodeDBContext : DbContext
    {
        public CodeDBContext(DbContextOptions<CodeDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }

        public DbSet<UserInfor> Users { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
