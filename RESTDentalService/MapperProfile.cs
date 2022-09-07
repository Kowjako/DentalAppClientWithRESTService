using AutoMapper;
using RESTDentalService.Entity;
using RESTDentalService.Models;

namespace RESTDentalService
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Clinic, ClinicDTO>()
                .ForMember(p => p.Manager, c => c.MapFrom(s => s.Manager.Name + " " + s.Manager.Surname));

            CreateMap<CreateClinicDTO, Clinic>()
                .ForMember(c => c.Manager, s => s.MapFrom(dto => new Employee()
                {
                    Name = dto.ManagerName,
                    Surname = dto.ManagerSurname,
                    Email = dto.ManagerEmail,
                    Phone = dto.ManagerPhone
                }));

            CreateMap<Employee, EmployeeDTO>()
                .ForMember(d => d.Clinic, s => s.MapFrom(dto => dto.ClinicNavigation));

            CreateMap<CreateEmployeeDTO, Employee>();

            CreateMap<Operation, OperationDTO>()
                .ForMember(d => d.Clinic, s => s.MapFrom(dto => dto.Clinic.Name))
                .ForMember(d => d.Doctor, s => s.MapFrom(dto => dto.Employee.Name + " " + dto.Employee.Surname))
                .ForMember(d => d.Term, s => s.MapFrom(dto => dto.Term.ToShortDateString()));
        }
    }
}
