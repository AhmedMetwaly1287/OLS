using System.Drawing.Text;
using OLS.Repository;
using OLS.Helpers;
using OLS.DTO;
using AutoMapper;
using OLS.Models;

namespace OLS.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }
        public async Task AddUser(RegisterDTO RegisterDTO)
        {
            var user = _mapper.Map<User>(RegisterDTO);
            await _userRepo.AddUser(user);

        }
        public async Task<string?> ValidateCreds(LoginDTO LoginDTO)
        {
            var User = _mapper.Map<User>(LoginDTO);
            return await _userRepo.ValidateCreds(User.Email,User.Password);
        }
        public async Task<bool> DeleteUser(int UserID)
        {
            return await _userRepo.DeleteUser(UserID);
        }
        public async Task<bool> ApproveUser(int UserID)
        {
            return await _userRepo.ApproveUser(UserID);
        }
        public async Task<bool> AlreadyApproved(int UserID)
        {
            return await _userRepo.AlreadyApproved(UserID);
        }
        public async Task<List<UserDTO>> GetAll()
        {
            var users = await _userRepo.GetAll(); 
            var usersDTO = users.Select(user => _mapper.Map<UserDTO>(user)).ToList();
            return usersDTO;
        }

        public async Task<UserDTO> GetUserByID(int userID) {
            var User = await _userRepo.GetUserByID(userID);
            var userDTO = _mapper.Map<UserDTO>(User);
            return userDTO;

        }
        public async Task<UserDTO> GetUserByEmail(string Email)
        {
            var User = await _userRepo.GetUserByEmail(Email);
            var userDTO = _mapper.Map<UserDTO>(User);
            return userDTO;

        }


        public async Task<bool> EmailExists(string Email)
        {
            return await _userRepo.EmailExists(Email);
        }
        public async Task<bool> UserExists(int ID) => await _userRepo.UserExists(ID);

        public string HashPassword(string Password)
        {
            return _userRepo.HashPassword(Password);

        }

        public async Task<bool> MakeLibrarian(int UserID)
        {
            return await _userRepo.MakeLibrarian(UserID);
        }
        public async Task<bool> CheckRole(int UserID)
        {
            return await _userRepo.CheckRole(UserID);
        }


    }
    }

