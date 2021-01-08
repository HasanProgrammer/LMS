using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model.Bases;
using Status = Model.Enums.Ticket.Status;

namespace Model
{
    public partial class Ticket : Entity
    {
        public string UserId  { get; set; } /*فرستنده Ticket*/
        public int? TermId    { get; set; }
        public string Content { get; set; }
        public string Answer  { get; set; }
        public Status Status  { get; set; }
    }
    
    public partial class Ticket
    {
        public virtual User User { get; set; }
        public virtual Term Term { get; set; }
    }
    
    public partial class Ticket : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");
            
            /*-------------------------------------------------------*/

            builder.HasKey(Ticket => Ticket.Id);
            
            /*-------------------------------------------------------*/

            builder.Property(Ticket => Ticket.Id)       .IsRequired();
            builder.Property(Ticket => Ticket.UserId)   .IsRequired();
            builder.Property(Ticket => Ticket.TermId)   .IsRequired();
            builder.Property(Ticket => Ticket.Content)  .IsRequired();
            builder.Property(Ticket => Ticket.Answer)   .IsRequired();
            builder.Property(Ticket => Ticket.Status)   .IsRequired().HasConversion(new EnumToNumberConverter<Status, int>());
            builder.Property(Ticket => Ticket.CreatedAt).IsRequired();
            builder.Property(Ticket => Ticket.UpdatedAt).IsRequired();
            
            /*-------------------------------------------------------*/

            builder.HasOne(Ticket => Ticket.User).WithMany(User => User.Tickets).HasForeignKey(Ticket => Ticket.UserId).OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(Ticket => Ticket.Term).WithMany(Term => Term.Tickets).HasForeignKey(Ticket => Ticket.TermId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}