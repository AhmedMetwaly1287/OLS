using OLS.Models;

namespace OLS.DTO
{
    public class AdminBorrowedBookDTO
    {
        public required int RequestID { get; set; }
        public required int UserID { get; set; }
        public string Email { get; set; }
        public required int BookID { get; set; }
        public string Title { get; set; }
        public required bool? IsApproved { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
       
}
