using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Bases;

namespace Model
{
    public partial class Answer : Entity
    {
        public string UserId  { get; set; }
        public int? CommentId { get; set; }
        public string Content { get; set; }
        public bool Show      { get; set; } = false;
    }
    
    public partial class Answer
    {
        public virtual User User       { get; set; }
        public virtual Comment Comment { get; set; }
    }
    
    public partial class Answer : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");
            
            /*-------------------------------------------------------*/

            builder.HasKey(Answer => Answer.Id);
            
            /*-------------------------------------------------------*/
            
            builder.Property(Answer => Answer.Id)       .IsRequired();
            builder.Property(Answer => Answer.UserId)   .IsRequired();
            builder.Property(Answer => Answer.CommentId).IsRequired();
            builder.Property(Answer => Answer.Content)  .IsRequired();
            builder.Property(Answer => Answer.CreatedAt).IsRequired();
            builder.Property(Answer => Answer.UpdatedAt).IsRequired();
            
            /*-------------------------------------------------------*/
            
            builder.HasOne(Answer => Answer.Comment).WithMany(Comment => Comment.Answers).HasForeignKey(Answer => Answer.CommentId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(Answer => Answer.User).WithMany(User => User.Answers).HasForeignKey(Answer => Answer.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}