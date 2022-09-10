using FluentValidation;
using RESTDentalService.Models;

namespace RESTDentalService.Validators
{
    public class RegisterUserDTOValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterUserDTOValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Imie nie moze byc puste");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Nazwisko nie może być puste");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Hasło nie może być puste");
        }
    }
}
