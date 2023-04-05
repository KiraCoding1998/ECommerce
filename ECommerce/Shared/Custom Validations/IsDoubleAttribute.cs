using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Custom_Validations
{
    public class IsDoubleAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            string valueInput  = (value as string)!;

            bool isNumeric = double.TryParse(valueInput, out _);
            return isNumeric;
        }
    }
}
