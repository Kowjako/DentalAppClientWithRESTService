using FluentValidation;
using RESTDentalService.Entity;
using RESTDentalService.Models;
using System;
using System.Linq;

namespace RESTDentalService.Validators
{
    public class CreateOperationDTOValidator : AbstractValidator<CreateOperationDTO>
    {
        public CreateOperationDTOValidator(DentalRestDbContext dbContext)
        {
            RuleFor(x => x.DoctorName).NotEmpty().WithMessage("Imię lekarza nie może być puste");
            RuleFor(x => x.DoctorSurname).NotEmpty().WithMessage("Nazwisko lekarza nie może być puste");

            RuleFor(x => x).Custom((obj, context) =>
            {
                var employee = dbContext.Employees.FirstOrDefault(p => p.Name.Equals(obj.DoctorName) &&
                                                                       p.Surname.Equals(obj.DoctorSurname));
                var clinic = dbContext.Clinics.FirstOrDefault(p => p.Id == obj.ClinicId);

                if(employee == null)
                {
                    context.AddFailure("Clinic", "Nie istnieje takigo pracownika");
                }

                if (clinic == null)
                {
                    context.AddFailure("Clinic", "Nie istnieje takiej przychodni");
                } 

                if (employee.ClinicId != clinic.Id)
                {
                    context.AddFailure("Employee", "Ten pracownik nie nalezy do tej przychodni");
                }
            });

            RuleFor(x => x.Cost).GreaterThan(0).WithMessage("Cena ma być wartością dodatnią");
            RuleFor(x => DateTime.Parse(x.Date)).GreaterThan(DateTime.Now).WithMessage("Dostępność operacji nie może być datą w przeszłości");
        }
    }
}
