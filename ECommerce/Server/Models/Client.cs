using ECommerce.Server.Services;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Server.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        public bool IsAdmin { get; set; } = false;

        public List<ClientProduct> ClientProducts { get; set; } = default!;

    }
}


