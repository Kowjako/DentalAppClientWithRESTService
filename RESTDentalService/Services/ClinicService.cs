using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RESTDentalService.Authorization;
using RESTDentalService.Entity;
using RESTDentalService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RESTDentalService.Services
{
    public interface IClinicService
    {
        Task<PagedResult<ClinicDTO>> GetAll(DentalAdvQuery query);
        Task<ClinicDTO> GetById(int id);
        Task<int> Create(CreateClinicDTO clinicDTO);
        Task Update(int id, UpdateClinicDTO dto);
        Task Delete(int id);
    }

    public class ClinicService : IClinicService
    {
        private readonly DentalRestDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ClinicService> _logger;
        private readonly IAuthorizationService _authService;
        private readonly IUserContextService _userService;

        public ClinicService(DentalRestDbContext context, IMapper mapper, ILogger<ClinicService> logger,
                             IAuthorizationService authService, IUserContextService userService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _authService = authService;
            _userService = userService;
        }

        public async Task<PagedResult<ClinicDTO>> GetAll(DentalAdvQuery query)
        {
            var baseQuery = _context.Clinics.Include(p => p.Manager)
                                            .Where(p => query.SearchPhrase == null ||
                                                   p.Name.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                                                   p.Location.ToLower().Contains(query.SearchPhrase.ToLower()));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var dictionary = new Dictionary<string, Expression<Func<Clinic, string>>>()
                {
                    { nameof(Clinic.Name), r => r.Name },
                    { nameof(Clinic.Location), r => r.Location},
                    { nameof(Clinic.UniqueNumber), r => r.UniqueNumber },
                };

                var sortExpression = dictionary[query.SortBy];

                baseQuery = query.SortDirection == SortType.Asc ?
                            baseQuery.OrderBy(sortExpression) :
                            baseQuery.OrderByDescending(sortExpression);
            }

            var clinics = await baseQuery.Skip((query.PageNumber - 1) * 30)
                                           .Take(30)
                                           .ToListAsync();

            var result = _mapper.Map<List<ClinicDTO>>(clinics);

            return new PagedResult<ClinicDTO>(result, baseQuery.Count(), 30, query.PageNumber);
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

            var authResult = await _authService.AuthorizeAsync(_userService.User, clinic, new AdminOrManagerRequirement());

            if (!authResult.Succeeded) throw new InvalidOperationException("Nie masz dostępu do tej akcji");

            _context.Clinics.Remove(clinic);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Przychodnia z Id = {clinic.Id} była usunięta");
        }
    }
}
