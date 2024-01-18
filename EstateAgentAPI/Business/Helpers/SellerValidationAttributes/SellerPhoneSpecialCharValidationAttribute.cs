using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Persistence.Models;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Buisness.Helpers.SellerValidationAttributes
{
    public class SellerPhoneSpecialCharValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value != null)
            {

                string phone = value.ToString();

                if (ContainsSpecialCharacters(phone))
                {
                    return new ValidationResult(ErrorMessage ?? "phone shouldn't consists of special characters");
                }
            }


            return ValidationResult.Success;

        }

        private bool ContainsSpecialCharacters(string phone)
        {
            return phone.Any(c => !char.IsLetterOrDigit(c) && !
            char.IsWhiteSpace(c));
        }


    }
}
