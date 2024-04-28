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
    public class BorrowedBookService : IBorrowedBookService
    {
        private readonly IBorrowedBookRepository _bbRepo;
        private readonly IArchiveService _archiveService;
        private readonly IMapper _mapper;
        public BorrowedBookService(IBorrowedBookRepository bbRepo,IArchiveService archiveService, IMapper mapper)
        {
            _bbRepo = bbRepo;
            _mapper = mapper;
            _archiveService = archiveService;
        }
        public async Task AddRequest([FromBody] RequestDTO RequestDTO )
        {
            var Request = _mapper.Map<BorrowedBook>( RequestDTO );
            await _bbRepo.AddRequest(Request);
        }
        public async Task<List<AdminBorrowedBookDTO>> GetAll()
        {
            var BorrowedBooks = await _bbRepo.GetAll();
            var BorrowedBooksDTO = BorrowedBooks.Select(borrowedbook => new AdminBorrowedBookDTO
            {
                RequestID = borrowedbook.RequestID,
                UserID = borrowedbook.UserID,
                Email = borrowedbook.User.Email,
                BookID = borrowedbook.BookID,
                IsApproved = borrowedbook.IsApproved,
                Title = borrowedbook.Book.Title,
                ReturnDate = borrowedbook.ReturnDate,
                BorrowedDate = borrowedbook.BorrowedDate
            }).ToList();
            return BorrowedBooksDTO;
        }
        public async Task<List<UserBorrowedBookDTO>> GetBorrowedBooksByID(int UserID)
        {
            var BorrowedBooks = await _bbRepo.GetBorrowedBooksByID(UserID);
            var BorrowedBooksDTO = BorrowedBooks.Select(borrowedbook => new UserBorrowedBookDTO
            {
                RequestID = borrowedbook.RequestID,
                BookID = borrowedbook.BookID,
                IsApproved = borrowedbook.IsApproved,
                Title = borrowedbook.Book.Title,
                ReturnDate = borrowedbook.ReturnDate
            }).Where(borrowedbook => borrowedbook.ReturnDate == null).ToList();
            return BorrowedBooksDTO;
        }

        public async Task<bool> RequestExists(int RequestID)
        {
            return await _bbRepo.RequestExists(RequestID);
        }
        public async Task<AdminBorrowedBookDTO> GetRequest(int RequestID)
        {
            var Request = await _bbRepo.GetRequest(RequestID);
            var ReqBorrowedBookDTO = _mapper.Map<AdminBorrowedBookDTO>(Request);
            return ReqBorrowedBookDTO;
        }
        public async Task<int> GetNumberOfRequests(int UserID)
        {
            return await _bbRepo.GetNumberOfRequests(UserID);
        }

        public async Task ApproveRequest(int RequestID)
        {
            await _bbRepo.ApproveRequest(RequestID);
        }

        public async Task RejectRequest(int RequestID)
        {
            await _bbRepo.RejectRequest(RequestID);
        }

        public async Task DeleteRequest(int RequestID)
        {
            await _bbRepo.DeleteRequest(RequestID);
        }
        public async Task ReturnBook(int RequestID)
        {
            await _bbRepo.ReturnBook(RequestID);
        }


    }
}

