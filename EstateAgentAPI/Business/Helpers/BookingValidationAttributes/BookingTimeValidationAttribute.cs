using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Business.Helpers.BookingValidationAttributes
{
    public class BookingTimeValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value != null)
            {

                var date = (DateTime?)value;
                if (date < DateTime.Now)
                {
                    return new ValidationResult("Date must be greater than or equal to today's date");
                }

            }
            return ValidationResult.Success;

        
    }
    }
}
