using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Models
{
    public class OwnedProductModel
    {
        public string ProductName { get; set; } = string.Empty;

        public string ProductDescription { get; set; } = string.Empty;

        public double ProductPrice { get; set; } 

        public int ClientHas { get; set; } = 0;

        public byte[]? ProductImage { get; set; }
    }
}
