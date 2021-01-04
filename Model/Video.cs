using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model.Bases;
using Status = Model.Enums.Video.Status;

namespace Model
{
    public partial class Video : Entity
    {
        public string UserId    { get; set; }
        public int? ChapterId   { get; set; }
        public int? TermId      { get; set; }
        public string VideoFile { get; set; }
        public bool IsFree      { get; set; } = false;
        public Status Status    { get; set; }
    }

    public partial class Video
    {
        public virtual User User       { get; set; }
        public virtual Chapter Chapter { get; set; }
        public virtual Term Term       { get; set; }
    }

    public partial class Video : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> builder)
        {
            builder.ToTable("Videos");
            
            /*-------------------------------------------------------*/

            builder.HasKey(Video => Video.Id);
            
            /*-------------------------------------------------------*/

            builder.Property(Video => Video.Id)       .IsRequired();
            builder.Property(Video => Video.UserId)   .IsRequired();
            builder.Property(Video => Video.TermId)   .IsRequired();
            builder.Property(Video => Video.VideoFile).IsRequired();
            builder.Property(Video => Video.Status)   .IsRequired().HasConversion(new EnumToNumberConverter<Status, int>());
            builder.Property(Video => Video.CreatedAt).IsRequired();
            builder.Property(Video => Video.UpdatedAt).IsRequired();
            
            /*-------------------------------------------------------*/
            
            builder.HasOne(Video => Video.Chapter).WithMany(Chapter => Chapter.Videos).HasForeignKey(Video => Video.ChapterId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(Video => Video.Term).WithMany(Term => Term.Videos).HasForeignKey(Video => Video.TermId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}