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
using Azure.Core;

namespace OLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowedBookController : ControllerBase
    {
        private readonly IBorrowedBookService _bbService;
        private readonly IBookService _bookService;
        private readonly IArchiveService _archiveService;
        private readonly int MAX_NUM = 2; //Max Number of Books to be borrowed

        public BorrowedBookController(IBorrowedBookService bbService, IBookService bookService, IArchiveService archiveService)
        {
            _bbService = bbService;
            _bookService = bookService;
            _archiveService = archiveService;
        }
        [HttpPost("SubmitRequest")]
        [Authorize(Policy ="IsApprovedPolicy")]
        public async Task<IActionResult> SubmitRequest([FromBody] RequestDTO RequestDTO)
        {
            if(await _bbService.GetNumberOfRequests(RequestDTO.UserID) == MAX_NUM)
            {
                return BadRequest("You have exceeded the allowed number of requests");
            }
            await _bbService.AddRequest(RequestDTO);
            await _bookService.TurnBorrowed(RequestDTO.BookID);
            return Ok(new{msg="Request Submitted Successfully, Please await the Librarians decision."});
        }

        [HttpGet("GetAll")]
        [Authorize(Policy = "RolePolicy")]
        public async Task<IActionResult> GetAllRequests()
        {
            var Requests = await _bbService.GetAll();
            return Ok(Requests);
        }

        [HttpGet("GetUserRequests/{UserID}")]
       [Authorize(Policy =("IsApprovedPolicy"))]
        public async Task<IActionResult> GetRequestsByID(int UserID)
        {
           var UserRequests = await _bbService.GetBorrowedBooksByID(UserID);
            return Ok(UserRequests);
        }

        [HttpPut("ApproveRequest/{ReqID}")]
        [Authorize(Policy =("RolePolicy"))]
        public async Task<IActionResult> ApproveRequest(int ReqID)
        {
                if(await _bbService.RequestExists(ReqID))
            {
                await _bbService.ApproveRequest(ReqID);
                return Ok(new {msg ="Request has been Approved"});
            }
            return NotFound("Request with this ID doesn't exist");
        }

        [HttpPut("RejectRequest/{ReqID}")]
        [Authorize(Policy =("RolePolicy"))]
        public async Task<IActionResult> RejectRequest(int ReqID)
        {
            if (await _bbService.RequestExists(ReqID))
            {
                var Req = await _bbService.GetRequest(ReqID);
                await _bbService.RejectRequest(ReqID);
                await _archiveService.AddToArchive(ReqID);
                await _bookService.TurnUnborrowed(Req.BookID);
                return Ok(new { msg = "Request has been Rejected" });
            }
            return NotFound("Request with this ID doesn't exist");
        }

        [HttpPut("ReturnBook/{ReqID}")]
        [Authorize(Policy = ("IsApprovedPolicy"))]
        public async Task<IActionResult> ReturnBook(int ReqID)
        {
            if (await _bbService.RequestExists(ReqID))
            {
                var Req = await _bbService.GetRequest(ReqID);
                await _bbService.ReturnBook(ReqID);
                await _archiveService.AddToArchive(ReqID);
                await _bookService.TurnUnborrowed(Req.BookID);
                return Ok("Book has been returned");
            }
            return NotFound("Request with this ID doesn't exist");
        }

        [HttpDelete("DeleteRequest/{ReqID}")]
        [Authorize(Policy = ("IsApprovedPolicy"))]
        public async Task<IActionResult> DeleteRequest(int ReqID)
        {
            if (await _bbService.RequestExists(ReqID))
            {
                var Req = await _bbService.GetRequest(ReqID);
                await _bbService.DeleteRequest(ReqID);
                await _bookService.TurnUnborrowed(Req.BookID);
                return Ok(new { msg = "Request has been deleted" });
            }
            return NotFound("Request with this ID doesn't exist");
        }

    }

}
