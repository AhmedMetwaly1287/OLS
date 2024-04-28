using System.Drawing.Text;
using OLS.Repository;
using OLS.Helpers;
using OLS.DTO;
using AutoMapper;
using OLS.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace OLS.Services
{
    public class ArchiveService : IArchiveService
    {
        private readonly IArchiveRepository _aRepo;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public ArchiveService(IArchiveRepository aRepo, IMapper mapper)
        {
            _aRepo = aRepo;
            _mapper = mapper;
        }
        public async Task AddToArchive(int BorrowedBookRequestID)
        {
            await _aRepo.AddToArchive(BorrowedBookRequestID); 
        }

        public async Task<List<AdminArchiveDTO>> GetAll()
        {
            var Archives = await _aRepo.GetAll();
            var ArchivesDTO = Archives.Select(Archive => new AdminArchiveDTO
            {
                ArchiveID = Archive.ArchiveID,
                BorrowedBookRequestID = Archive.BorrowedBooks.RequestID,
                UserID = Archive.BorrowedBooks.UserID,
                Email = Archive.BorrowedBooks.User.Email,
                BookID = Archive.BorrowedBooks.BookID,
                IsApproved = Archive.BorrowedBooks.IsApproved,
                Title = Archive.BorrowedBooks.Book.Title,
                BorrowedDate = Archive.BorrowedBooks.BorrowedDate,
                ReturnDate = Archive.BorrowedBooks.ReturnDate,
            }).ToList();
            return ArchivesDTO;
        }
        public async Task<List<UserArchiveDTO>> GetArchiveByID(int UserID)
        {
            var Archives = await _aRepo.GetArchiveByID(UserID);
            var ArchivesDTO = Archives.Select(Archive => new UserArchiveDTO
            {
                ArchiveID = Archive.ArchiveID,
                BorrowedBookRequestID = Archive.BorrowedBooks.RequestID,
                BookID = Archive.BorrowedBooks.BookID,
                Title = Archive.BorrowedBooks.Book.Title,
                IsApproved = Archive.BorrowedBooks.IsApproved,
                ReturnDate = Archive.BorrowedBooks.ReturnDate,
            }).ToList();
            return ArchivesDTO; ;
        }


    }
}

