using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Bases;

namespace Model
{
    public partial class Comment : Entity
    {
        public string UserId  { get; set; }
        public int? TermId    { get; set; }
        public string Title   { get; set; }
        public string Content { get; set; }
        public bool Show      { get; set; } = false;
    }
    
    public partial class Comment
    {
        public virtual User User { get; set; }
        public virtual Term Term { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public virtual ICollection<Answer> Answers { get; set; }
    }
    
    public partial class Comment : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            
            /*-------------------------------------------------------*/

            builder.HasKey(Comment => Comment.Id);
            
            /*-------------------------------------------------------*/

            builder.Property(Comment => Comment.Id)       .IsRequired();
            builder.Property(Comment => Comment.UserId)   .IsRequired();
            builder.Property(Comment => Comment.TermId)   .IsRequired();
            builder.Property(Comment => Comment.Title)    .IsRequired();
            builder.Property(Comment => Comment.Content)  .IsRequired();
            builder.Property(Comment => Comment.CreatedAt).IsRequired();
            builder.Property(Comment => Comment.UpdatedAt).IsRequired();
            
            /*-------------------------------------------------------*/
            
            builder.HasOne(Comment => Comment.User).WithMany(User => User.Comments).HasForeignKey(Comment => Comment.UserId).OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(Comment => Comment.Term).WithMany(Term => Term.Comments).HasForeignKey(Comment => Comment.TermId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(Comment => Comment.Answers).WithOne(Answer => Answer.Comment).HasForeignKey(Answer => Answer.CommentId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}