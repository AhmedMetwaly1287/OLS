using OLS.Models;
using OLS.DTO;
using Microsoft.AspNetCore.Mvc;

namespace OLS.Services
{
    public interface IArchiveService
    {
        Task AddToArchive(int BorrowedBookRequestID);
        Task<List<AdminArchiveDTO>> GetAll();
        Task<List<UserArchiveDTO>> GetArchiveByID(int UserID);
    }
}
