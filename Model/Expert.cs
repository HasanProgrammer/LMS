using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Model
{
    public partial class Expert
    {
        public int? Id     { get; set; }
        public string Name { get; set; }
    }

    public partial class Expert
    {
        public virtual ICollection<ExpertUser> ExpertUsers { get; set; }
    }

    public partial class Expert : IEntityTypeConfiguration<Expert>
    {
        public void Configure(EntityTypeBuilder<Expert> builder)
        {
            builder.ToTable("Experts");
            
            /*-------------------------------------------------------*/

            builder.HasKey(Expert => Expert.Id);
            
            /*-------------------------------------------------------*/

            builder.Property(Expert => Expert.Id)  .IsRequired();
            builder.Property(Expert => Expert.Name).IsRequired();
            
            /*-------------------------------------------------------*/
            
            
        }
    }
}