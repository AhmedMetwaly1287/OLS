using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OLS.Models
{
    public class BorrowedBook
    {
        [Key]
        [JsonIgnore]
        public required int RequestID { get; set; }

        public required int UserID { get; set; }
        public required User User { get; set; } 

        public required int BookID { get; set; }
        public required Book Book { get; set; }

        [JsonIgnore]
        public DateTime BorrowedDate { get; set; } = DateTime.Now;

        [JsonIgnore]
        public DateTime? ReturnDate { get; set; }

        public required bool? IsApproved { get; set; }
    }
}
