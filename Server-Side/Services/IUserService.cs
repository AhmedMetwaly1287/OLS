using OLS.Models;
using OLS.DTO;

namespace OLS.Services
{
    public interface IUserService
    {
        Task AddUser(RegisterDTO RegisterDTO);
        Task<string?> ValidateCreds(LoginDTO LoginDTO);
        Task<bool> DeleteUser(int UserID);
        Task<List<UserDTO>> GetAll();
        Task<UserDTO> GetUserByID(int id);
        Task<UserDTO> GetUserByEmail(string Email);
        Task<bool> EmailExists(string email);
        Task<bool> UserExists(int id);
        Task<bool> ApproveUser(int id);
        Task<bool> AlreadyApproved(int id);
        Task<bool> MakeLibrarian(int UserID);
        Task<bool> CheckRole(int UserID);
        string HashPassword(string password);
    }
}
