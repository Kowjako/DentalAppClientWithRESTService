using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RESTDentalService.Entity;
using RESTDentalService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTDentalService.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDTO>> GetAll();
        Task<EmployeeDTO> GetById(int id);
        Task<int> Create(CreateEmployeeDTO empDTO);
        Task DeleteById(int id);
        Task Update(int id, UpdateEmployeeDTO dto);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly DentalRestDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeService(DentalRestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDTO>> GetAll()
        {
            var baseQuery = await _context.Employees.Include(p => p.ClinicNavigation).ToListAsync();

            var result = _mapper.Map<List<EmployeeDTO>>(baseQuery);
            return result;
        }

        public async Task<EmployeeDTO> GetById(int id)
        {
            var employee = await _context.Employees.Include(p => p.Clinic).FirstOrDefaultAsync(p => p.Id == id);

            if (employee == null) throw new ArgumentNullException("Takiego pracownika nie istnieje");
            return _mapper.Map<EmployeeDTO>(employee);
        }

        public async Task<int> Create(CreateEmployeeDTO empDTO)
        {
            var employee = _mapper.Map<Employee>(empDTO);

            var clinic = await _context.Clinics.FirstOrDefaultAsync(p => p.UniqueNumber.Equals(empDTO.ClinicUniqueNumber));

            employee.ClinicId = clinic.Id;
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return clinic.Id;
        }

        public async Task DeleteById(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(p => p.Id == id);
            if(employee == null) throw new ArgumentNullException("Takiego pracownika nie istnieje");

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, UpdateEmployeeDTO dto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) throw new ArgumentNullException("Takiego pracownika nie istnieje");

            employee.Name = dto.Name;
            employee.Surname = dto.Surname;
            employee.Phone = dto.Phone;
            employee.Email = dto.Email;

            await _context.SaveChangesAsync();
        }
    }
}
