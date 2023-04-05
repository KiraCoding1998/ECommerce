namespace ECommerce.Server.Models
{
    public class ClientProduct
    {

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int ClientId { get; set; }
        public Models.Client? Client { get; set; }

        public int ClientHas { get; set; } = 0;
    }
}
