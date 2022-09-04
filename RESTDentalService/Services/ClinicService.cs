using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RESTDentalService.Entity;
using RESTDentalService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTDentalService.Services
{
    public interface IClinicService
    {
        Task<List<ClinicDTO>> GetAll();
        Task<ClinicDTO> GetById(int id);
        Task<int> Create(CreateClinicDTO clinicDTO);
        Task Update(int id, UpdateClinicDTO dto);
        Task Delete(int id);
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

        public async Task<ClinicDTO> GetById(int id)
        {
            var clinic = await _context.Clinics.Include(p => p.Manager).FirstOrDefaultAsync(p => p.Id == id);

            if (clinic == null) throw new ArgumentNullException("Takiej przychodni nie istnieje");
            return _mapper.Map<ClinicDTO>(clinic);
        }

        public async Task<int> Create(CreateClinicDTO clinicDTO)
        {
            var clinic = _mapper.Map<Clinic>(clinicDTO);

            var existingEmployee = await _context.Employees.FirstOrDefaultAsync(p => p.Name == clinicDTO.ManagerName &&
                                                                               p.Surname == clinicDTO.ManagerSurname &&
                                                                               p.Email == clinicDTO.ManagerEmail);
            if(existingEmployee != null)
            {
                clinic.ManagerId = existingEmployee.Id;
                clinic.Manager = null;
            }

            _context.Clinics.Add(clinic);
            await _context.SaveChangesAsync();
            return clinic.Id;
        }

        public async Task Update(int id, UpdateClinicDTO dto)
        {
            var clinic = await _context.Clinics.FindAsync(id);
            if (clinic == null) throw new ArgumentNullException("Takiej przychodni nie istnieje");

            clinic.Name = dto.Name;
            clinic.Location = dto.Location;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var clinic = await _context.Clinics.FindAsync(id);
            if (clinic == null) throw new ArgumentNullException("Takiej przychodni nie istnieje");

            _context.Clinics.Remove(clinic);
            await _context.SaveChangesAsync();
        }
    }
}
