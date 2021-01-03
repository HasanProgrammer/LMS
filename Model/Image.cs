using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Bases;

namespace Model
{
    public partial class Image : Entity
    {
        public string Path   { get; set; }
        public string Type   { get; set; }
    }
    
    public partial class Image
    {
        public virtual User User { get; set; }
    }
    
    public partial class Image : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Images");
            
            /*---------------------------------------------------*/

            builder.HasKey(Image => Image.Id);
            
            /*---------------------------------------------------*/

            builder.Property(Image => Image.Id)       .IsRequired();
            builder.Property(Image => Image.Path)     .IsRequired();
            builder.Property(Image => Image.Type)     .IsRequired();
            builder.Property(Image => Image.CreatedAt).IsRequired();
            builder.Property(Image => Image.UpdatedAt).IsRequired();
            
            /*---------------------------------------------------*/

            builder.HasOne(Image => Image.User).WithOne(User => User.Image);
        }
    }
}