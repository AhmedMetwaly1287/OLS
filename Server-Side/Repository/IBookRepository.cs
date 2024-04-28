using OLS.Models;

namespace OLS.Repository
{
    public interface IBookRepository
    {
        Task AddBook(Book Book);
        Task<bool> DeleteBook(int BookID);
        Task<List<Book>> GetAll();
        Task<Book> GetBookByID(int BookID);
        Task<List<Book>> GetBooksByISBN(string partialISBN);
        Task<List<Book>> GetBooksByRN(string partialRN);
        Task<List<Book>> GetBooksByTitle(string partialTitle);
        Task<List<Book>> GetBooksByAuthor(string partialAuthor);
        Task<bool> ISBNExists(string ISBN);
        Task<bool> BookExists(int ID);
        Task TurnBorrowed(int BookID);
        Task TurnUnborrowed(int BookID);
        Task<bool> IsBorrowed(int BookID);
        Task UpdateISBN(int BookID, string newISBN);
        Task UpdateRackNum(int BookID, string newRN);
        Task UpdateTitle(int BookID, string newTitle);
        Task UpdateAuthor(int BookID, string newAuthor);
    }
}
