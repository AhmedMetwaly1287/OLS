using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace OLS.Models
{
    public class Book
    {
        [Key]
        [JsonIgnore]
        public required int ID {  get; set; }
                
        [MaxLength(50)]
        public required string ISBN { get; set; }

        [MaxLength(50)]
        public required string RackNumber {  get; set; }

        [MaxLength(50)]
        public required string Author { get; set; }

        [MaxLength(50)]
        public required string Title { get; set; }

        [DefaultValue(false)]
        public required bool IsBorrowed { get; set; }

        [JsonIgnore]
        public required DateTime CreationDate { get; set; } = DateTime.Today;



    }
}
