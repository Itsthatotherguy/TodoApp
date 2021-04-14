using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Models.Attributes
{
    public class RequiredDateAttribute : ValidationAttribute
    {
        public string GetErrorMessage() => "Please provide a date";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(GetErrorMessage());
            }

            var date = (DateTime)value;

            if (date == DateTime.MinValue)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
