using Microsoft.EntityFrameworkCore;
using OLS.Data;
using OLS.Models;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OLS.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;
        public BookRepository(AppDbContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;

        }

        public async Task AddBook(Book Book)
        {
                await _db.Books.AddAsync(Book);
                await _db.SaveChangesAsync();   
        }

        public async Task<bool> DeleteBook(int BookID)
        {
            var Book = await GetBookByID(BookID);
            if (await BookExists(BookID))
            {
                _db.Books.Remove(Book);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<List<Book>> GetAll()
        {
            return await _db.Books.ToListAsync();
        }
        public async Task<Book> GetBookByID(int BookID) =>  await _db.Books.FindAsync(BookID);

        public async Task<List<Book>> GetBooksByISBN(string partialISBN)
        {
            return await _db.Books.Where(b => b.ISBN.Contains(partialISBN)).ToListAsync();
        }

        public async Task<List<Book>> GetBooksByRN(string partialRN)
        {
            return await _db.Books.Where(b => b.RackNumber.Contains(partialRN)).ToListAsync();
        }

        public async Task<List<Book>> GetBooksByTitle(string partialTitle)
        {
            return await _db.Books.Where(b => b.Title.Contains(partialTitle)).ToListAsync();
        }

        public async Task<List<Book>> GetBooksByAuthor(string partialAuthor)
        {
            return await _db.Books.Where(b => b.Author.Contains(partialAuthor)).ToListAsync();
        }


        public async Task<bool> ISBNExists(string ISBN)
        {
            return await _db.Books.AnyAsync(b => b.ISBN == ISBN);
        }
        public async Task<bool> BookExists(int ID) => await _db.Books.AnyAsync(b => b.ID == ID);

        public async Task TurnBorrowed(int BookID)
        {
            var Book = await GetBookByID(BookID);
                Book.IsBorrowed = true;
                _db.SaveChanges();
           
        }

        public async Task TurnUnborrowed(int BookID)
        {
            var Book = await GetBookByID(BookID);
            Book.IsBorrowed = false;
            _db.SaveChanges();

        }

        public async Task<bool> IsBorrowed(int BookID)
        {
            var Book = await GetBookByID(BookID);
            return Book.IsBorrowed == true ? true : false;
        }

        public async Task UpdateISBN(int BookID,string newISBN)
        {
            var Book = await GetBookByID(BookID);
            Book.ISBN = newISBN;
            _db.SaveChanges();
        }
        public async Task UpdateRackNum(int BookID, string newRN)
        {
            var Book = await GetBookByID(BookID);
            Book.RackNumber = newRN;
            _db.SaveChanges();
        }
        public async Task UpdateTitle(int BookID, string newTitle)
        {
            var Book = await GetBookByID(BookID);
            Book.Title = newTitle;
            _db.SaveChanges();
        }
        public async Task UpdateAuthor(int BookID, string newAuthor)
        {
            var Book = await GetBookByID(BookID);
            Book.Author = newAuthor;
            _db.SaveChanges();
        }

    }
}
