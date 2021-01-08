using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model.Bases;
using Status = Model.Enums.Term.Status;

namespace Model
{
    public partial class Term : Entity
    {
        public string UserId      { get; set; }
        public int? ImageId       { get; set; }
        public int? CategoryId    { get; set; }
        public string Name        { get; set; }
        public string Description { get; set; }
        public string Suitable    { get; set; }
        public string Result      { get; set; }
        public int? Price         { get; set; }
        public bool HasChapter    { get; set; } = false;
        public Status Status      { get; set; }
    }

    public partial class Term
    {
        public virtual User User         { get; set; }
        public virtual Image Image       { get; set; }
        public virtual Category Category { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public virtual ICollection<Buy> Buys                 { get; set; }
        public virtual ICollection<Video> Videos             { get; set; }
        public virtual ICollection<Chapter> Chapters         { get; set; }
        public virtual ICollection<Comment> Comments         { get; set; }
        public virtual ICollection<Ticket>  Tickets          { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }

    public partial class Term : IEntityTypeConfiguration<Term>
    {
        public void Configure(EntityTypeBuilder<Term> builder)
        {
            builder.ToTable("Terms");
            
            /*-------------------------------------------------------*/

            builder.HasKey(Term => Term.Id);
            
            /*-------------------------------------------------------*/

            builder.Property(Term => Term.Id)         .IsRequired();
            builder.Property(Term => Term.UserId)     .IsRequired();
            builder.Property(Term => Term.ImageId)    .IsRequired();
            builder.Property(Term => Term.CategoryId) .IsRequired();
            builder.Property(Term => Term.Name)       .IsRequired();
            builder.Property(Term => Term.Description).IsRequired();
            builder.Property(Term => Term.Suitable)   .IsRequired();
            builder.Property(Term => Term.Result)     .IsRequired();
            builder.Property(Term => Term.Price)      .IsRequired();
            builder.Property(Term => Term.Status)     .IsRequired().HasConversion(new EnumToNumberConverter<Status, int>());
            builder.Property(Term => Term.CreatedAt)  .IsRequired();
            builder.Property(Term => Term.UpdatedAt)  .IsRequired();
            
            /*-------------------------------------------------------*/

            builder.HasMany(Term => Term.Chapters).WithOne(Chapter => Chapter.Term).HasForeignKey(Chapter => Chapter.TermId).OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(Term => Term.Category).WithMany(Category => Category.Terms).HasForeignKey(Term => Term.CategoryId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(Term => Term.Image).WithOne(Image => Image.Term).HasForeignKey<Term>(Term => Term.ImageId);

            builder.HasOne(Term => Term.User).WithMany(User => User.Terms).HasForeignKey(Term => Term.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(Term => Term.Videos).WithOne(Video => Video.Term).HasForeignKey(Video => Video.TermId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(Term => Term.Comments).WithOne(Comment => Comment.Term).HasForeignKey(Comment => Comment.TermId).OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(Term => Term.Tickets).WithOne(Ticket => Ticket.Term).HasForeignKey(Ticket => Ticket.TermId).OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(Term => Term.Buys).WithOne(Buy => Buy.Term).HasForeignKey(Buy => Buy.TermId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(Term => Term.Transactions).WithOne(Tran => Tran.Term).HasForeignKey(Tran => Tran.TermId);
        }
    }
}