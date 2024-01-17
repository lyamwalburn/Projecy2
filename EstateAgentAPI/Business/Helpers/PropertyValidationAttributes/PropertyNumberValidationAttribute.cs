using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Persistence.Models;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Business.Helpers.SellerValidationAttributes
{
    public class PropertyNumberValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value != null)
            {

                int number=  (int) value;
                

                if (number < 1 )
                {
                    return new ValidationResult(ErrorMessage ?? "Bathroom/Bedroom can't be less than 1");
                }
            }


            return ValidationResult.Success;

        }
        




    }
}
