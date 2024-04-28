namespace OLS.DTO
{
    public class AdminArchiveDTO
    {
        public required int ArchiveID {  get; set; }
        public required int BorrowedBookRequestID { get; set; }
        public int UserID { get; set; }
        public string Email { get; set; }
        public  int BookID { get; set; }
        public string Title { get; set; }
        public  bool? IsApproved { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
