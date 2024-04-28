using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using OLS.Models;

namespace OLS.Data{
    public class AppDbContext : DbContext{
        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BorrowedBook> BorrowedBooks { get; set; }

        public DbSet<Archive> Archives { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.Email })
                .IsUnique();

            modelBuilder.Entity<Book>()
                .HasIndex(b => new { b.ISBN })
                .IsUnique();

            modelBuilder.Entity<BorrowedBook>()
                .HasOne(bb => bb.User)
                .WithMany()
                .HasForeignKey(bb => bb.UserID) //A User can borrow up to 5 books (One-To-Many relationship)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BorrowedBook>()
                .HasOne(bb => bb.Book)
                .WithMany()
                .HasForeignKey(bb => bb.BookID) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Archive>()
                .HasOne(a => a.BorrowedBooks)
                .WithOne()
                .HasForeignKey<Archive>(a => a.BorrowedBookRequestID).OnDelete(DeleteBehavior.Cascade); //One To One Relationship between BorrowedBook and Archive

        }


    }
}