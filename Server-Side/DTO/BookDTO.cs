namespace OLS.DTO
{
    public class BookDTO
    {
        public required int ID { get; set; }

        public required string ISBN { get; set; }

        public required string RackNumber { get; set; }

        public required string Author { get; set; }

        public required string Title { get; set; }
        public required bool IsBorrowed { get; set; }
    }
}
