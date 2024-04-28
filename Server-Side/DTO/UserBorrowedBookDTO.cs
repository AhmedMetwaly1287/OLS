using OLS.Models;

namespace OLS.DTO
{
    public class UserBorrowedBookDTO
    {
        public required int RequestID { get; set; }
        public required int BookID { get; set; }
        public string Title { get; set; }
        public required bool? IsApproved { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
       
}
