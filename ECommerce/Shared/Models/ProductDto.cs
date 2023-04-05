using ECommerce.Shared.Custom_Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Models
{
    public class ProductDto
    {
        public int Id { get; set; }

        public byte[]? ProductImage { get; set; } = default!;

        public string? Description { get; set; } = string.Empty;


        //[RegularExpression(@"^[0-9]\d*(\.\d+)?$",ErrorMessage ="Invalid Input")]
        public double Price { get; set; } = 0.0;

        [Required(ErrorMessage = " Don't you have a name ?")]
        public string ProductName { get; set; } = string.Empty;
        
        //[RegularExpression(@"^[0-9]*$",ErrorMessage ="Nigger enter an integer")]
        public int PiecesAvaliable { get; set; } = 0;


    }
}
