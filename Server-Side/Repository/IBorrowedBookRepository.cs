using Microsoft.AspNetCore.Mvc;
using OLS.Models;

namespace OLS.Repository
{
    public interface IBorrowedBookRepository
    {
        Task AddRequest([FromBody] BorrowedBook borrowedBook);
          Task<List<BorrowedBook>> GetAll();
          Task<List<BorrowedBook>> GetBorrowedBooksByID(int UserID);
          Task<bool> RequestExists(int RequestID);
          Task<BorrowedBook> GetRequest(int RequestID);
          Task<int> GetNumberOfRequests(int UserID);
          Task ApproveRequest(int RequestID);
          Task RejectRequest(int RequestID);
          Task DeleteRequest(int RequestID);
          Task ReturnBook(int RequestID);
    }
}
