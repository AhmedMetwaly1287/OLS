using Microsoft.AspNetCore.Mvc;
using OLS.Data;
using OLS.Repository;
using OLS.Models;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using OLS.DTO;
using OLS.Services;
using OLS.Authorization;
using AutoMapper;

namespace OLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
           _bookService = bookService;
        }
        [HttpGet("GetAll")]
        [Authorize(Policy = "IsApprovedPolicy")]
        public async Task<IActionResult> GetAllBooks()
        {
            var Books = await _bookService.GetAll();
            return Ok(Books);
        }

        [HttpGet("GetBook/{id}")]
       [Authorize(Policy =("IsApprovedPolicy"))]
        //Might need more Adjustments role Wise
        public async Task<IActionResult> GetBookByID(int id)
        {
            if(await _bookService.BookExists(id))
            {
                var BookInfo = await _bookService.GetBookByID(id);
                return Ok(BookInfo);
            }
            return NotFound($"Book with the ID {id} does not exist");
        }

        [HttpGet("FilterByISBN/{ISBN}")]
         [Authorize(Policy =("IsApprovedPolicy"))]
        //Might need more Adjustments role Wise
        public async Task<IActionResult> GetBookByISBN(string ISBN)
        {
                var BookInfo = await _bookService.GetBooksByISBN(ISBN);
                return Ok(BookInfo);
        }

        [HttpGet("FilterByRN/{RN}")]
        [Authorize(Policy =("IsApprovedPolicy"))]
        //Might need more Adjustments role Wise
        public async Task<IActionResult> GetBooksByRN(string RN)
        {
            var BookInfo = await _bookService.GetBooksByRN(RN);
            return Ok(BookInfo);
        }

        [HttpGet("FilterByTitle/{title}")]
        [Authorize(Policy = ("IsApprovedPolicy"))]
        public async Task<IActionResult> GetBooksByTitle(string title)
        {
            var BookInfo = await _bookService.GetBooksByTitle(title);
            return Ok(BookInfo);
        }

        [HttpGet("FilterByAuthor/{author}")]
        [Authorize(Policy = ("IsApprovedPolicy"))]
        public async Task<IActionResult> GetBooksByAuthor(string author)
        {
            var BookInfo = await _bookService.GetBooksByAuthor(author);
            return Ok(BookInfo);
        }

        [HttpGet("CheckISBN/{ISBN}")]
        [Authorize(Policy = ("RolePolicy"))]
        //Might need more Adjustments role Wise
        public async Task<bool> CheckISBN(string ISBN)
        {
            if(await _bookService.ISBNExists(ISBN))
            {
                return true;
            }
            return false;
        }



        [HttpPost("AddBook")]
       [Authorize(Policy ="RolePolicy")]
        public async Task<IActionResult> AddBook([FromBody] AddBookDTO BookDTO)
        {
 
            if (await _bookService.ISBNExists(BookDTO.ISBN))
            {
                return BadRequest("A Book with this ISBN Already Exists");
            }

            await _bookService.AddBook(BookDTO);

            return Ok(new{msg = "New Book Added"});
        }
        
  
        [HttpPut("TurnBorrowed/{id}")]
        [Authorize(Policy =("IsApprovedPolicy"))]
        public async Task<IActionResult> TurnBorrowed(int id) 
        {
            if(await _bookService.BookExists(id) && !await _bookService.IsBorrowed(id))
            {
                await _bookService.TurnBorrowed(id);
                return Ok(new { msg = $"Book with ID {id} has been made borrowed" });
            }
            return BadRequest("Error while making book borrowed (Book is already borrowed or it doesn't exist)");
        }

        [HttpPut("TurnUnborrowed/{id}")]
        [Authorize(Policy =("IsApprovedPolicy"))]
        public async Task<IActionResult> TurnUnborrowed(int id)
        {
            if (await _bookService.BookExists(id) && await _bookService.IsBorrowed(id))
            {
                await _bookService.TurnUnborrowed(id);
                return Ok(new { msg = $"Book with ID {id} has been made Unborrowed" });
            }
            return BadRequest("Error while making book Unborrowed (Book is already Unborrowed or it doesn't exist)");
        }

        [HttpDelete("DeleteBook/{id}")]
       [Authorize(Policy = ("RolePolicy"))]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if(await _bookService.BookExists(id))
            {
                await _bookService.DeleteBook(id);
                return Ok(new { msg = $"Book with the ID {id} has been deleted successfully" });
            }
            return NotFound($"Book with the ID {id} does not exist");
        }

        [HttpPatch("UpdateBook/{id}")]
        [Authorize(Policy = ("RolePolicy"))]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDTO BookDTO)
        {
            var existingBook = await _bookService.GetBookByID(id);
            if (!await _bookService.BookExists(id))
            {
                return NotFound(); 
            }
            if (BookDTO.ISBN != null && !await _bookService.ISBNExists(BookDTO.ISBN))
            {
                await _bookService.UpdateISBN(id, BookDTO);
            }
            if (BookDTO.RackNumber != null)
            {
                await _bookService.UpdateRackNum(id, BookDTO);
            }

            if (BookDTO.Title != null)
            {
                await _bookService.UpdateTitle(id, BookDTO);
            }
            if (BookDTO.Author != null)
            {
                await _bookService.UpdateAuthor(id, BookDTO);
            }

            return Ok(new { msg =  "Book Updated Successfully"});
        }



    }

}
