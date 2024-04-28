using OLS.Models;
using OLS.DTO;

namespace OLS.Services
{
    public interface IBookService
    {
        Task AddBook(AddBookDTO BookDTO);
        Task<bool> DeleteBook(int BookID);
        Task<List<BookDTO>> GetAll();
        Task<BookDTO> GetBookByID(int BookID);
        Task<List<BookDTO>> GetBooksByISBN(string partialISBN);
        Task<List<BookDTO>> GetBooksByRN(string partialRN);
        Task<List<BookDTO>> GetBooksByTitle(string partialTitle);
        Task<List<BookDTO>> GetBooksByAuthor(string partialAuthor);
        Task<bool> ISBNExists(string ISBN);
        Task<bool> BookExists(int ID);
        Task TurnBorrowed(int BookID);
        Task TurnUnborrowed(int BookID);
        Task<bool> IsBorrowed(int BookID);
        Task UpdateISBN(int BookID, UpdateBookDTO BookDTO);
        Task UpdateRackNum(int BookID, UpdateBookDTO BookDTO);
        Task UpdateTitle(int BookID, UpdateBookDTO BookDTO);
        Task UpdateAuthor(int BookID, UpdateBookDTO BookDTO);
    }
}
