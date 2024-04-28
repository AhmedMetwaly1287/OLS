using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using static System.Reflection.Metadata.BlobBuilder;
namespace OLS.Models
{
    public class User
    {
        [Key]
        [JsonIgnore]
        public int ID { get; set; }

        [MaxLength(50)]
        public required string Email { get; set; }

        [MaxLength(50)]
        public required string FName { get; set; }

        [MaxLength(50)]
        public required string LName { get; set; }

        [MaxLength(255)]
        public required string Password { get; set; }

        [JsonIgnore]
        public int RoleID { get; set; } = 0; //0 for Normal User, 1 for Libraian 

        [DefaultValue(false)]
        [JsonIgnore]
        public bool IsApproved { get; set; }

        [JsonIgnore]
        public DateTime CreationDate { get; set; } = DateTime.Now;

    }
}
