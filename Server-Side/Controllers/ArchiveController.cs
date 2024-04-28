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
    public class ArchiveController : ControllerBase
    {
        private readonly IArchiveService _aService;


        public ArchiveController(IArchiveService aService)
        {
            _aService = aService;
        }
        [HttpGet("GetAll")]
        [Authorize(Policy = "RolePolicy")]
        public async Task<IActionResult> GetAllArchives()
        {
            var Archives = await _aService.GetAll();
            return Ok(Archives);
        }

        [HttpGet("GetUserArchive/{UserID}")]
        [Authorize(Policy = ("IsApprovedPolicy"))]
        public async Task<IActionResult> GetUserArchives(int UserID)
        {
            var UserRequests = await _aService.GetArchiveByID(UserID);
            return Ok(UserRequests);
        }


    }

}
