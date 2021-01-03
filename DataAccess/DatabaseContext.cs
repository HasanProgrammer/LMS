using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccess
{
    /*Config*/
    public partial class DatabaseContext : IdentityDbContext<User, Role, string,
                                                                         IdentityUserClaim<string>, /*Not Me*/
                                                                         UserRole,
                                                                         IdentityUserLogin<string>, /*Not Me*/
                                                                         IdentityRoleClaim<string>, /*Not Me*/
                                                                         IdentityUserToken<string>> /*Not Me*/
    {
        public DatabaseContext(DbContextOptions Options) : base(Options)
        {
            
        }
    }
    
    /*Entity*/
    public partial class DatabaseContext
    {
        public DbSet<Category> Categories { get; set; }
    }
    
    /*Relation*/
    public partial class DatabaseContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new User());
            builder.ApplyConfiguration(new Role());
            builder.ApplyConfiguration(new UserRole());
            builder.ApplyConfiguration(new Category());
        }
    }
}