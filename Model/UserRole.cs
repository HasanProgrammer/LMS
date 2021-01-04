using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Model
{
    /*Property | Fields*/
    public partial class UserRole : IdentityUserRole<string>
    {
        
    }

    /*Navigation Property | Relations in code*/
    public partial class UserRole
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
    
    /*Relations | Config*/
    public partial class UserRole : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(UR => new { UR.UserId , UR.RoleId });
            
            /*---------------------------------------------------*/
                
            builder.HasOne(UR => UR.Role).WithMany(Role => Role.UserRoles).HasForeignKey(UR => UR.RoleId).OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(UR => UR.User).WithMany(User => User.UserRoles).HasForeignKey(UR => UR.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}