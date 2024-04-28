namespace OLS.DTO
{
    public class AddArchiveDTO
    {
        public required int ArchiveID {  get; set; }
        public required int BorrowedBookRequestID { get; set; }
    }
}
