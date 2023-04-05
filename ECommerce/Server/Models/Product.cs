using System.ComponentModel.DataAnnotations;

namespace ECommerce.Server.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public byte[]? ProductImage { get; set; } = default!;
        

        public string? Description { get; set; } = string.Empty;

        public double Price { get; set; } = 0.0;

        [Required]
        public string ProductName { get; set; } =string.Empty;

        public int PiecesAvaliable { get; set; }

        public List<ClientProduct> ClientProducts { get; set; } = default!;
    }

}
