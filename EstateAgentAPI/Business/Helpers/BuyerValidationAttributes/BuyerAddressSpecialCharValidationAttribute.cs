using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Persistence.Models;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Business.Helpers.BuyerValidationAttributes
{
    public class BuyerAddressSpecialCharValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value != null)
            {

                string address = value.ToString();
               
                

                if (ContainsSpecialCharacters(address) )
                {
                    return new ValidationResult(ErrorMessage ?? "Address shouldn't consists of special characters");
                }
            }


            return ValidationResult.Success;

        }

        private bool ContainsSpecialCharacters(string address)
        {
            return address.Any(c => !char.IsLetterOrDigit(c) && !
            char.IsWhiteSpace(c));
        }


    }
}
