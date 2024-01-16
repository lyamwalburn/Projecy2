using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Persistence.Models;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Buisness.Helpers.SellerValidationAttributes
{
    public class SellerNameSpecialCharValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value != null)
            {

                string firstName = value.ToString();
                string lastName = value.ToString();
                if (ContainsSpecialCharacters(firstName) && ContainsSpecialCharacters(lastName))
                {
                    return new ValidationResult(ErrorMessage ?? "Name shouldn't consists of special characters");
                }

            }
              return ValidationResult.Success;

        }

        private bool ContainsSpecialCharacters(string sellerName)
        {
            return sellerName.Any(c => !char.IsLetterOrDigit(c) && !
            char.IsWhiteSpace(c));
        }


    }
}
