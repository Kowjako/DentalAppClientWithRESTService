using FluentValidation;
using RESTDentalService.Entity;
using RESTDentalService.Models;
using System.Linq;

namespace RESTDentalService.Validators
{
    public class CreateEmployeeDTOValidator : AbstractValidator<CreateEmployeeDTO>
    {
        public CreateEmployeeDTOValidator(DentalRestDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Imię nie może być puste");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Nazwisko nie może być puste");
            RuleFor(x => x.Phone).NotEmpty().MaximumLength(25).WithMessage("Numer nie może być pusty ani większy niż 25 znaków");
            RuleFor(x => x.Email).NotEmpty().MaximumLength(25).WithMessage("Email nie może być pusty ani większy niż 25 znaków");

            RuleFor(x => x).Custom((value, context) =>
            {
                if (dbContext.Employees.Any(p => p.Name == value.Name &&
                                                 p.Surname == value.Surname &&
                                                 p.Email == value.Email))
                {
                    context.AddFailure("CreateEmployeeDTO", "Imie, Nazwisko i Email muszą być unikalne");
                }
            });

            RuleFor(x => x.ClinicUniqueNumber).Custom((value, context) =>
            {
                if (!dbContext.Clinics.Any(p => p.UniqueNumber.Equals(value)))
                {
                    context.AddFailure("Nie istnieje takiej przychodni");
                }
            });
        }
    }
}
