using EstateAgentAPI.Business.DTO;
using EstateAgentAPI.Persistence.Models;
using System.ComponentModel.DataAnnotations;

namespace EstateAgentAPI.Buisness.Helpers.BuyerValidationAttributes
{
    public class BuyerNameValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value != null)
            {

                string firstName = value.ToString();
                string lastName = value.ToString();

                if (!IsAlphabetical(firstName) && !IsAlphabetical(lastName))
                {
                    return new ValidationResult(ErrorMessage ?? "Name should consists of alphabets");
                }

            }

            return ValidationResult.Success;

        }
        private bool IsAlphabetical(string name)
        {
            return name.All(char.IsLetter);

        }


        
    }
}
