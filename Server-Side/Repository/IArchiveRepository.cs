using Microsoft.AspNetCore.Mvc;
using OLS.Models;

namespace OLS.Repository
{
    public interface IArchiveRepository
    {
        Task AddToArchive(int BorrowedBookRequestID);
        Task<List<Archive>> GetAll();
        Task<List<Archive>> GetArchiveByID(int UserID);

    }
}
