using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookingApp.Validation
{
    public class ComboBoxValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || (int)value == -1)
            {
                return new ValidationResult(false, "*");
            }
            return ValidationResult.ValidResult;
        }
    }
}
