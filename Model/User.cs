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
        public int? ImageId           { get; set; }
        public string Phone           { get; set; }
        public string EmailCode       { get; set; }
        public int PhoneCode          { get; set; }
        public bool IsVerifyEmail     { get; set; } = false;
        public bool IsVerifyPhone     { get; set; } = false;
        public Status Status          { get; set; }
        public long CreatedAtTimeStamp { get; set; }
        public string CreatedAt       { get; set; }
        public string UpdatedAt       { get; set; }
    }
    
    /*Navigation Property | Relations*/
    public partial class User
    {
        public virtual Image Image                     { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
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
            
            builder.HasOne(User => User.Image)     .WithOne(Image => Image.User).HasForeignKey<User>(User => User.ImageId);
            builder.HasMany(User => User.UserRoles).WithOne(UR => UR.User)      .OnDelete(DeleteBehavior.Cascade);
        }
    }
}