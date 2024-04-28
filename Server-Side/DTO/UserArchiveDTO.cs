namespace OLS.DTO
{
    public class UserArchiveDTO
    {
        public required int ArchiveID {  get; set; }
        public required int BorrowedBookRequestID { get; set; }

        public  int BookID { get; set; }
        public string Title { get; set; }
        public  bool? IsApproved { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
