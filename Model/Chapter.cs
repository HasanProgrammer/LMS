using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Bases;

namespace Model
{
    public partial class Chapter : Entity
    {
        public string UserId { get; set; }
        public int? TermId   { get; set; }
        public string Title  { get; set; }
    }

    public partial class Chapter
    {
        public virtual User User { get; set; }
        public virtual Term Term { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public virtual ICollection<Video> Videos { get; set; }
    }

    public partial class Chapter : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.ToTable("Chapters");
            
            /*-------------------------------------------------------*/

            builder.HasKey(Chapter => Chapter.Id);
            
            /*-------------------------------------------------------*/

            builder.Property(Chapter => Chapter.Id)       .IsRequired();
            builder.Property(Chapter => Chapter.UserId)   .IsRequired();
            builder.Property(Chapter => Chapter.TermId)   .IsRequired();
            builder.Property(Chapter => Chapter.Title)    .IsRequired();
            builder.Property(Chapter => Chapter.CreatedAt).IsRequired();
            builder.Property(Chapter => Chapter.UpdatedAt).IsRequired();
            
            /*-------------------------------------------------------*/

            builder.HasMany(Chapter => Chapter.Videos).WithOne(Video => Video.Chapter).HasForeignKey(Video => Video.ChapterId).OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(Chapter => Chapter.User).WithMany(User => User.Chapters).HasForeignKey(Chapter => Chapter.UserId).OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(Chapter => Chapter.Term).WithMany(Term => Term.Chapters).HasForeignKey(Chapter => Chapter.TermId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}