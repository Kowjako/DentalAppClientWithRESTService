using FluentValidation;
using RESTDentalService.Entity;
using RESTDentalService.Models;
using System.Linq;

namespace RESTDentalService.Validators
{
    public class CreateClinicDTOValidator : AbstractValidator<CreateClinicDTO>
    {
        public CreateClinicDTOValidator(DentalRestDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(30).WithMessage("Nazwa nie moze byc pusta ani wieksza niz 30 znakow");
            RuleFor(x => x.Location).NotEmpty().MaximumLength(25).WithMessage("Lokacja nie moze byc pusta ani wieksza niz 25 znakow");
            RuleFor(x => x.ManagerSurname).NotEmpty().WithMessage("Nazwisko kierownika nie moze byc puste");
            RuleFor(x => x.ManagerName).NotEmpty().WithMessage("Imie kierownika nie moze byc puste");
            RuleFor(x => x.ManagerPhone).NotEmpty().WithMessage("Numer kierownika nie moze byc pusty");
            RuleFor(x => x.ManagerEmail).NotEmpty().WithMessage("Email kierownika nie moze byc pusty");

            RuleFor(x => x.UniqueNumber).Custom((value, context) =>
            {
                if (dbContext.Clinics.Any(p => p.UniqueNumber.Equals(value)))
                {
                    context.AddFailure("UniqueNumber", "Numer przychodni musi byc unikalny");
                }
                if (value.Length != 10)
                {
                    context.AddFailure("UniqueNumber", "Numer musi się składać z 10 znaków");
                }
            });
        }
    }
}
