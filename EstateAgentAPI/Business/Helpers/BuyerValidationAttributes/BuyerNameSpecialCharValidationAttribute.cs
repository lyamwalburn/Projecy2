using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Persistence.Models;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Buisness.Helpers.BuyerValidationAttributes
{
    public class BuyerNameSpecialCharValidationAttribute : ValidationAttribute
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

        private bool ContainsSpecialCharacters(string buyerName)
        {
            return buyerName.Any(c => !char.IsLetterOrDigit(c) && !
            char.IsWhiteSpace(c));
        }


    }
}
