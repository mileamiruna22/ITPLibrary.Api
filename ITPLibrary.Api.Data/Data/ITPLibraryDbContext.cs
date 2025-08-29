using ITPLibrary.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITPLibrary.Api.Data.Data
{
    public class ITPLibraryDbContext : DbContext
    {
        
        public ITPLibraryDbContext() { }

        public ITPLibraryDbContext(DbContextOptions<ITPLibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=ITPLibraryDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False");
            }
        }
    }
}