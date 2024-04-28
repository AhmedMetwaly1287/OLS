namespace OLS.DTO
{
    public class UpdateBookDTO
    {

        public required string ISBN { get; set; }

        public required string RackNumber { get; set; }

        public required string Author { get; set; }

        public required string Title { get; set; }
    }
}
