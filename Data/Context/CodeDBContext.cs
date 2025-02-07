using CodeLeapChallengeAPI_06022025.Data.Class;
using Microsoft.EntityFrameworkCore;

namespace CodeLeapChallengeAPI_06022025.Data.Context
{
    /// <summary>
    /// 
    /// </summary>
    public class CodeDBContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public CodeDBContext(DbContextOptions<CodeDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<UserInfor> Users { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Product> Products { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<ErrorLog> ErrorLogs { get; set; }
    }
}
