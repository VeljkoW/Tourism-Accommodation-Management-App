using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookingApp.View.Guide.Validators
{
    public class IntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value as string, out _))
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Please enter a valid integer.");
        }
    }
}
