using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Persistence.Models;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Business.Helpers.SellerValidationAttributes
{
    public class PropertyTypeValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value != null)
            {

                string type = value.ToString();
               
                

                if (ContainsSpecialCharacters(type) && IsAlphabetical(type))
                {
                    return new ValidationResult(ErrorMessage ?? "Type shouldn't consists of numbers or special characters");
                }
            }


            return ValidationResult.Success;

        }

        private bool ContainsSpecialCharacters(string type)
        {
            return type.Any(c => !char.IsLetterOrDigit(c) && !
            char.IsWhiteSpace(c));
        }
        private bool IsAlphabetical(string type)
        {
            return type.All(char.IsLetter);

        }


    }
}
