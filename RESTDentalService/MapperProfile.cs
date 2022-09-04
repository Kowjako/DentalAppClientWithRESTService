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
        }
    }
}
