using API.Contracts;
using System.ComponentModel.DataAnnotations;

namespace API.Utilities
{

    // Not used
    public class ValidationOtpAttribute : ValidationAttribute
    {
        private readonly string _email;
        public ValidationOtpAttribute(string email)
        {
            _email = email;
        }
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return ValidationResult.Success;

            var employeeRepository = (IAccountRepository)validationContext.GetService(typeof(IAccountRepository))!;
            var entity = employeeRepository.CheckOtp(_email, int.Parse(value.ToString()));
            if (entity is null)
            {
                return new ValidationResult("Otp doesn't match");
            }
            if (entity.IsUsed == true)
            {
                return new ValidationResult("Otp has been used");
            }
            if (entity.ExpiredTime > DateTime.Now)
            {
                return new ValidationResult("Otp alredy expired");
            }
            return ValidationResult.Success;
        }
    }
}
