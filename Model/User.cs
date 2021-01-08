using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model.Bases;
using Status = Model.Enums.User.Status;

namespace Model
{
    /*Property | Fields*/
    public partial class User : IdentityUser<string>, IEntity
    {
        public int? ImageId            { get; set; }
        public string Phone            { get; set; }
        public string EmailCode        { get; set; }
        public int? PhoneCode          { get; set; }
        public bool IsVerifyEmail      { get; set; } = false;
        public bool IsVerifyPhone      { get; set; } = false;
        public Status Status           { get; set; }
        public long CreatedAtTimeStamp { get; set; }
        public string CreatedAt        { get; set; }
        public string UpdatedAt        { get; set; }
    }
    
    /*Navigation Property | Relations*/
    public partial class User
    {
        public virtual Image Image { get; set; }
        
        /*-----------------------------------------------------------*/
        
        public virtual ICollection<Buy> Buys                 { get; set; }
        public virtual ICollection<Term> Terms               { get; set; }
        public virtual ICollection<Video> Videos             { get; set; }
        public virtual ICollection<Chapter> Chapters         { get; set; }
        public virtual ICollection<Answer> Answers           { get; set; }
        public virtual ICollection<Comment> Comments         { get; set; }
        public virtual ICollection<UserRole> UserRoles       { get; set; }
        public virtual ICollection<Ticket> Tickets           { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
    
    /*Relation | Configs*/
    public partial class User : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
                
            /*---------------------------------------------------*/
    
            builder.HasKey(User => User.Id);
                
            /*---------------------------------------------------*/
    
            builder.Property(User => User.Id)                .IsRequired();
            builder.Property(User => User.Email)             .IsRequired();
            builder.Property(User => User.Phone)             .IsRequired();
            builder.Property(User => User.Status)            .IsRequired().HasConversion(new EnumToNumberConverter<Status, int>());
            builder.Property(User => User.CreatedAtTimeStamp).IsRequired();
            builder.Property(User => User.CreatedAt)         .IsRequired();
            builder.Property(User => User.UpdatedAt)         .IsRequired();
                
            /*---------------------------------------------------*/
            
            builder.HasOne(User => User.Image).WithOne(Image => Image.User).HasForeignKey<User>(User => User.ImageId);
            
            builder.HasMany(User => User.UserRoles).WithOne(UR => UR.User).HasForeignKey(UR => UR.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(User => User.Chapters).WithOne(Chapter => Chapter.User).HasForeignKey(Chapter => Chapter.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(User => User.Terms).WithOne(Term => Term.User).HasForeignKey(Term => Term.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(User => User.Videos).WithOne(Video => Video.User).HasForeignKey(Video => Video.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(User => User.Comments).WithOne(Comment => Comment.User).HasForeignKey(Comment => Comment.UserId).OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(User => User.Answers).WithOne(Answer => Answer.User).HasForeignKey(Answer => Answer.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(User => User.Tickets).WithOne(Ticket => Ticket.User).HasForeignKey(Ticket => Ticket.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(User => User.Buys).WithOne(Buy => Buy.User).HasForeignKey(Buy => Buy.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(User => User.Transactions).WithOne(Tran => Tran.User).HasForeignKey(Tran => Tran.UserId);
        }
    }
}