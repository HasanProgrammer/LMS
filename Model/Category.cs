using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Bases;

namespace Model
{
    public partial class Category : Entity
    {
        public string Name { get; set; }
    }

    public partial class Category
    {
        
    }

    public partial class Category : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            
            /*---------------------------------------------------*/

            builder.HasKey(Category => Category.Id);
            
            /*---------------------------------------------------*/

            builder.Property(Category => Category.Id)       .IsRequired();
            builder.Property(Category => Category.Name)     .IsRequired();
            builder.Property(Category => Category.CreatedAt).IsRequired();
            builder.Property(Category => Category.UpdatedAt).IsRequired();
        }
    }
}