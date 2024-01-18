using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Persistence.Models;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Business.Helpers.BuyerValidationAttributes
{
    public class BuyerPostCodeSpecialCharValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value != null)
            {

                string postcode = value.ToString();

                if (ContainsSpecialCharacters(postcode))
                {
                    return new ValidationResult(ErrorMessage ?? "Postcode shouldn't consists of special characters");
                }
            }


            return ValidationResult.Success;

        }

        private bool ContainsSpecialCharacters(string postcode)
        {
            return postcode.Any(c => !char.IsLetterOrDigit(c) && !
            char.IsWhiteSpace(c));
        }


    }
}
