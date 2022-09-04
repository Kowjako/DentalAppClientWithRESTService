using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RESTDentalService.Entity;
using RESTDentalService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTDentalService.Services
{
    public interface IClinicService
    {
        Task<List<ClinicDTO>> GetAll();
    }

    public class ClinicService : IClinicService
    {
        private readonly DentalRestDbContext _context;
        private readonly IMapper _mapper;

        public ClinicService(DentalRestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ClinicDTO>> GetAll()
        {
            var baseQuery = await _context.Clinics.Include(p => p.Manager).ToListAsync();

            var result = _mapper.Map<List<ClinicDTO>>(baseQuery);
            return result;
        }
    }
}
