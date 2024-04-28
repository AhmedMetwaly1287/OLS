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
    public class ArchiveRepository : IArchiveRepository
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;
        public ArchiveRepository(AppDbContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;

        }
        public async Task AddToArchive(int BorrowedBookRequestID)
        {
            var archive = new Archive
            {
                BorrowedBookRequestID = BorrowedBookRequestID
            };
            _db.Archives.Add(archive);
            await _db.SaveChangesAsync();
        }


        public async Task<List<Archive>> GetAll()
        {
            return await _db.Archives
                        .Include(a => a.BorrowedBooks)
                        .ThenInclude(a => a.User)
                        .Include(a => a.BorrowedBooks)
                        .ThenInclude(a => a.Book)
                        .ToListAsync();
        }
        public async Task<List<Archive>> GetArchiveByID(int UserID)
        {
            return await _db.Archives
                    .Include(a => a.BorrowedBooks)
                    .ThenInclude(a => a.Book)
                    .Include(a => a.BorrowedBooks)
                    .ThenInclude(a=>a.User)
                    .Where(a => a.BorrowedBooks.UserID == UserID)
                    .ToListAsync();
        }
    }
}
