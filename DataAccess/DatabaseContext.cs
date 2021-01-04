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
        public DbSet<Answer> Answers      { get; set; }
        public DbSet<Buy> Buys            { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Chapter> Chapters    { get; set; }
        public DbSet<Comment> Comments    { get; set; }
        public DbSet<Image> Images        { get; set; }
        public DbSet<Term> Terms          { get; set; }
        public DbSet<Ticket> Tickets      { get; set; }
        public DbSet<Video> Videos        { get; set; }
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
            builder.ApplyConfiguration(new Answer());
            builder.ApplyConfiguration(new Buy());
            builder.ApplyConfiguration(new Category());
            builder.ApplyConfiguration(new Chapter());
            builder.ApplyConfiguration(new Comment());
            builder.ApplyConfiguration(new Image());
            builder.ApplyConfiguration(new Term());
            builder.ApplyConfiguration(new Ticket());
            builder.ApplyConfiguration(new Video());
        }
    }
}