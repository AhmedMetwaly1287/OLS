using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace OLS.Models
{
    public class Archive
    {
        [Key]
        [JsonIgnore]
        public int ArchiveID { get; set; }

        public required int BorrowedBookRequestID {  get; set; }

        public BorrowedBook BorrowedBooks { get; set; } //To Retrieve the Borrowed Date (User and Book info too)

    }
}
