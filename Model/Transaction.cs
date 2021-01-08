using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model.Bases;
using Status = Model.Enums.Transaction.Status;

namespace Model
{
    public partial class Transaction : Entity
    {
        public string UserId     { get; set; }
        public int? TermId       { get; set; }
        public string RefId      { get; set; } /*این کد، مربوطه به پیگیری تراکنش مالی می باشد*/
        public int? Price        { get; set; }
        public string UserPhone  { get; set; }
        public string UserEmail  { get; set; }
        public Status Status     { get; set; }
        public int? StatusCode   { get; set; }
    }

    public partial class Transaction
    {
        public virtual User User { get; set; }
        public virtual Term Term { get; set; }
    }

    public partial class Transaction : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transactions");
            
            /*-------------------------------------------------------*/

            builder.HasKey(Tran => Tran.Id);
            
            /*-------------------------------------------------------*/
            
            builder.Property(Tran => Tran.Id)        .IsRequired();
            builder.Property(Tran => Tran.UserId)    .IsRequired();
            builder.Property(Tran => Tran.TermId)    .IsRequired();
            builder.Property(Tran => Tran.RefId)     .IsRequired();
            builder.Property(Tran => Tran.Price)     .IsRequired();
            builder.Property(Tran => Tran.StatusCode).IsRequired();
            builder.Property(Tran => Tran.UserPhone) .IsRequired();
            builder.Property(Tran => Tran.UserEmail) .IsRequired();
            builder.Property(Tran => Tran.Status)    .IsRequired().HasConversion(new EnumToNumberConverter<Status, int>());
            builder.Property(Tran => Tran.CreatedAt) .IsRequired();
            builder.Property(Tran => Tran.UpdatedAt) .IsRequired();
            
            /*-------------------------------------------------------*/

            builder.HasOne(Tran => Tran.User).WithMany(User => User.Transactions).HasForeignKey(Tran => Tran.UserId);
            
            builder.HasOne(Tran => Tran.Term).WithMany(Term => Term.Transactions).HasForeignKey(Tran => Tran.TermId);
        }
    }
}