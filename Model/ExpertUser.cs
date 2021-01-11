using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Model
{
    public partial class ExpertUser
    {
        public string UserId { get; set; }
        public int ExpertId  { get; set; }
    }

    public partial class ExpertUser
    {
        public virtual User User     { get; set; }
        public virtual Expert Expert { get; set; }
    }

    public partial class ExpertUser : IEntityTypeConfiguration<ExpertUser>
    {
        public void Configure(EntityTypeBuilder<ExpertUser> builder)
        {
            
        }
    }
}