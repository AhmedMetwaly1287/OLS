using Microsoft.EntityFrameworkCore;
using OLS.Data;
using OLS.Models;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;

namespace OLS.Repository
{
    public class BorrowedBookRepository : IBorrowedBookRepository
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;
        //isApproved = 0 & ReturnDate != NULL, Rejected, isApproved = 1 & ReturnDate != NULL, Returned
        public BorrowedBookRepository(AppDbContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;

        }
        public async Task AddRequest([FromBody] BorrowedBook borrowedBook)
        {
            _db.BorrowedBooks.Add(borrowedBook);
            await _db.SaveChangesAsync();
        }

        public async Task<List<BorrowedBook>> GetAll()
        {
            return await _db.BorrowedBooks
                        .Include(bb => bb.User)
                        .Include(bb => bb.Book) 
                        .ToListAsync();
        }
        public async Task<List<BorrowedBook>> GetBorrowedBooksByID(int UserID)
        {
            return await _db.BorrowedBooks
                        .Include(bb => bb.User)
                        .Include(bb => bb.Book)
                        .Where(bb => bb.User.ID == UserID)
                        .ToListAsync();
        }
        public async Task<bool> RequestExists(int RequestID)
        {
            return await _db.BorrowedBooks.AnyAsync(bb => bb.RequestID == RequestID);
        }
        public async Task<BorrowedBook> GetRequest(int RequestID) 
        {
            return await _db.BorrowedBooks.FindAsync(RequestID);
        }
        public async Task<int> GetNumberOfRequests(int UserID)
        {
            return await _db.BorrowedBooks
                    .Where(bb => bb.UserID == UserID && bb.ReturnDate == null)
                    .CountAsync();
        }

       public async Task ApproveRequest(int RequestID)
        {
            var Request = await GetRequest(RequestID);
            Request.IsApproved = true; //Return Date remains NULL until the user returns the book
            await _db.SaveChangesAsync();
        }

        public async Task RejectRequest(int RequestID)
        {
            //TO-DO: REMOVE FROM BORROWED BOOK, SEND TO ARCHIVE
            var Request = await GetRequest(RequestID);
            Request.IsApproved = false;
            Request.ReturnDate = DateTime.Now; //Won't be sent to the BorrowedBook User page, Displays Status as Rejected in Archive
            await _db.SaveChangesAsync();
        }

        public async Task DeleteRequest(int RequestID)
        {
            if(await RequestExists(RequestID))
            {
                //Never Happened (Invoked by user Cancellation)
                var Req = await GetRequest(RequestID);
                _db.BorrowedBooks.Remove(Req);
                await _db.SaveChangesAsync();
            }
        }

        public async Task ReturnBook(int RequestID)
        {
            var Request = await GetRequest(RequestID);
            Request.IsApproved = true;
            Request.ReturnDate = DateTime.Now; 
            
            await _db.SaveChangesAsync();
        }
    }
}
