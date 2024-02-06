using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ToDoList.Models
{
    [Index(nameof(UserName), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        [StringLength(30)]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(30)]
        public string LastName { get; set; } = string.Empty;
        [StringLength(30)]
        public string UserName { get; set; } = string.Empty;
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        [JsonIgnore]
        [StringLength(255)]
        public byte[] PasswordHash { get; set; } = Array.Empty<Byte>();
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; } = Array.Empty<Byte>();
        public bool isAdmin { get; set; } = false;
    }
}
