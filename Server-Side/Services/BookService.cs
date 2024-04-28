using System.Drawing.Text;
using OLS.Repository;
using OLS.Helpers;
using OLS.DTO;
using AutoMapper;
using OLS.Models;

namespace OLS.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepo;
        private readonly IMapper _mapper;
        public BookService(IBookRepository bookRepo, IMapper mapper)
        {
            _bookRepo = bookRepo;
            _mapper = mapper;
        }
        public async Task AddBook(AddBookDTO BookDTO)
        {
            var Book = _mapper.Map<Book>(BookDTO);
            await _bookRepo.AddBook(Book);

        }

        public async Task<bool> DeleteBook(int BookID)
        {
            return await _bookRepo.DeleteBook(BookID);
        }
 
        public async Task<List<BookDTO>> GetAll()
        {
            var Books = await _bookRepo.GetAll(); 
            var BookDTO = Books.Select(book => _mapper.Map<BookDTO>(book)).ToList();
            return BookDTO;
        }

        public async Task<BookDTO> GetBookByID(int BookID) {
            var Book = await _bookRepo.GetBookByID(BookID);
            var BookDTO = _mapper.Map<BookDTO>(Book);
            return BookDTO;

        }

        public async Task<List<BookDTO>> GetBooksByISBN(string partialISBN)
        {
            var Books = await _bookRepo.GetBooksByISBN(partialISBN);
            var BookDTOs = Books.Select(book => _mapper.Map<BookDTO>(book)).ToList();
            return BookDTOs;
        }


        public async Task<List<BookDTO>> GetBooksByRN(string partialRN)
        {
            var Books = await _bookRepo.GetBooksByRN(partialRN);
            var BookDTOs = Books.Select(book => _mapper.Map<BookDTO>(book)).ToList();
            return BookDTOs;
        }

        public async Task<List<BookDTO>> GetBooksByTitle(string partialTitle)
        {
            var Books = await _bookRepo.GetBooksByTitle(partialTitle);
            var BookDTOs = Books.Select(book => _mapper.Map<BookDTO>(book)).ToList();
            return BookDTOs;
        }

        public async Task<List<BookDTO>> GetBooksByAuthor(string partialAuthor)
        {
            var Books = await _bookRepo.GetBooksByAuthor(partialAuthor);
            var BookDTOs = Books.Select(book => _mapper.Map<BookDTO>(book)).ToList();
            return BookDTOs;
        }


        public async Task<bool> ISBNExists(string ISBN)
        {
            return await _bookRepo.ISBNExists(ISBN);
        }
        public async Task<bool> BookExists(int ID) => await _bookRepo.BookExists(ID);

        public async Task TurnBorrowed(int BookID)
        {
            await _bookRepo.TurnBorrowed(BookID);

        }

        public async Task TurnUnborrowed(int BookID)
        {
            await _bookRepo.TurnUnborrowed(BookID);
        }

        public async Task<bool> IsBorrowed(int BookID)
        {
            return await _bookRepo.IsBorrowed(BookID);
        }

        public async Task UpdateISBN(int BookID, UpdateBookDTO BookDTO)
        {
            await _bookRepo.UpdateISBN(BookID, BookDTO.ISBN);
        }
        public async Task UpdateRackNum(int BookID, UpdateBookDTO BookDTO)
        {
            await _bookRepo.UpdateRackNum(BookID, BookDTO.RackNumber);
        }
        
        public async Task UpdateTitle(int BookID, UpdateBookDTO BookDTO)
        {
            await _bookRepo.UpdateTitle(BookID, BookDTO.Title);
        }
        public async Task UpdateAuthor(int BookID, UpdateBookDTO BookDTO)
        {
            await _bookRepo.UpdateAuthor(BookID, BookDTO.Author);
        }



    }
}

