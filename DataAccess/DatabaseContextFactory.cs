using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess
{
    /*در این قسمت برای آن که EFCore بتواند Context ما را شناسایی کند تا فایل های Migration مربوطه را بسازد ، باید از دستورات زیر استفاده کنیم*/
    /*این کار برای آن است که ما می خواهیم فایل های Migration خود را در لایه ای جدا از لایه اصلی که فایل StartUp در آن است ، ایجاد نماییم*/
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=LMS;Trusted_Connection=True;");
            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}