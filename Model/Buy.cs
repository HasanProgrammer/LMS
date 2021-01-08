using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Bases;

namespace Model
{
    public partial class Buy : IViewEntity
    {
        public int? Id          { get; set; }
        public string UserId    { get; set; }
        public int?   TermId    { get; set; }
        public string CreatedAt { get; set; }
    }
    
    public partial class Buy
    {
        public virtual User User { get; set; }
        public virtual Term Term { get; set; }
    }
    
    public partial class Buy : IEntityTypeConfiguration<Buy>
    {
        public void Configure(EntityTypeBuilder<Buy> builder)
        {
            builder.ToTable("Purchases");
            
            /*-------------------------------------------------------*/

            builder.HasKey(Buy => Buy.Id);
            
            /*-------------------------------------------------------*/

            builder.Property(Buy => Buy.Id)       .IsRequired();
            builder.Property(Buy => Buy.UserId)   .IsRequired();
            builder.Property(Buy => Buy.TermId)   .IsRequired();
            builder.Property(Buy => Buy.CreatedAt).IsRequired();
            
            /*-------------------------------------------------------*/

            builder.HasOne(Buy => Buy.User).WithMany(User => User.Buys).HasForeignKey(Buy => Buy.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(Buy => Buy.Term).WithMany(Term => Term.Buys).HasForeignKey(Buy => Buy.TermId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}