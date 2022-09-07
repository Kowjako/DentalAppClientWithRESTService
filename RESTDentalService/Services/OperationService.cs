using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RESTDentalService.Entity;
using RESTDentalService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RESTDentalService.Services
{
    public interface IOperationService
    {
        Task<PagedResult<OperationDTO>> GetAll(int clinicId, DentalAdvQuery query);
        Task<int> Create(int clinidId, CreateOperationDTO operation);
        Task DeleteById(int id);
        Task DeleteAllForClinic(int clinicId);
    }

    public class OperationService : IOperationService
    {
        private readonly DentalRestDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OperationService> _logger;

        public OperationService(DentalRestDbContext context, IMapper mapper, ILogger<OperationService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Create(int clinicId, CreateOperationDTO operation)
        {
            var employee = await _context.Employees.FirstAsync(p => p.Name.Equals(operation.DoctorName) &&
                                                              p.Surname.Equals(operation.DoctorSurname));

            var clinic = await _context.Clinics.FirstOrDefaultAsync(p => p.Id == clinicId);
            if (clinic == null) throw new ArgumentNullException("Takiej przychodni nie istnieje");

            var operationDb = _mapper.Map<Operation>(operation);
            operationDb.ClinicId = clinic.Id;
            operationDb.EmployeeId = employee.Id;

            _context.Operations.Add(operationDb);
            await _context.SaveChangesAsync();
            return operationDb.Id;
        }

        public async Task DeleteAllForClinic(int clinicId)
        {
            var clinic = await _context.Clinics.FirstOrDefaultAsync(p => p.Id == clinicId);
            if (clinic == null) throw new ArgumentNullException("Takiej przychodni nie istnieje");

            _context.Operations.RemoveRange(_context.Operations.Where(p => p.ClinicId == clinicId));
            _logger.LogInformation($"Wszystkie operacje były usunięte dla przychodni Id = {clinicId}");
        }

        public async Task DeleteById(int id)
        {
            var operation = await _context.Operations.FirstOrDefaultAsync(p => p.Id == id);
            if (operation == null) throw new ArgumentNullException("Takiej operacji nie istnieje");

            _context.Operations.Remove(operation);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Operacja z Id = {operation.Id} była usunięta");
        }

        public async Task<PagedResult<OperationDTO>> GetAll(int clinicId, DentalAdvQuery query)
        {
            var baseQuery = _context.Operations.Include(p => p.Employee)
                                               .Include(p => p.Clinic)
                                               .Where(p => query.SearchPhrase == null ||
                                                     p.Name.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                                                     p.Clinic.Name.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                                                     p.Employee.Name.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                                                     p.Employee.Surname.ToLower().Contains(query.SearchPhrase.ToLower()));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var dictionary = new Dictionary<string, Expression<Func<Operation, object>>>()
                {
                    { nameof(Operation.Cost), r => r.Cost },
                    { nameof(Operation.Term), r => r.Term },
                    { nameof(Operation.Name), r => r.Name },
                };

                var sortExpression = dictionary[query.SortBy];

                baseQuery = query.SortDirection == SortType.Asc ?
                            baseQuery.OrderBy(sortExpression) :
                            baseQuery.OrderByDescending(sortExpression);
            }

            var clinics = await baseQuery.Where(p => p.ClinicId == clinicId)
                                         .Skip((query.PageNumber - 1) * 30)
                                         .Take(30)
                                         .ToListAsync();

            var result = _mapper.Map<List<OperationDTO>>(clinics);

            return new PagedResult<OperationDTO>(result, baseQuery.Count(), 30, query.PageNumber);
        }
    }
}
