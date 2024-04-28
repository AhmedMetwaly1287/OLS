using OLS.Models;

namespace OLS.DTO
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string Email { get; set; }

        public string FName { get; set; }

        public string LName { get; set; }
        public int RoleID { get; set; }
        public bool IsApproved { get; set; }
    }
}
