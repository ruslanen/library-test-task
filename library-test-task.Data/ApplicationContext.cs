using library_test_task.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace library_test_task.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookStorage> BookStorage { get; set; }
        
        public DbSet<BookRent> BookRent { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
    }
}