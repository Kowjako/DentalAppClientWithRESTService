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
        Task<PagedResult<OperationDTO>> GetAll(DentalAdvQuery query);
        Task<int> Create(CreateOperationDTO operation);
        Task DeleteById(int id);
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

        public async Task<int> Create(CreateOperationDTO operation)
        {

        }

        public async Task DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<OperationDTO>> GetAll(DentalAdvQuery query)
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

            var clinics = await baseQuery.Skip((query.PageNumber - 1) * 30)
                                         .Take(30)
                                         .ToListAsync();

            var result = _mapper.Map<List<OperationDTO>>(clinics);

            return new PagedResult<OperationDTO>(result, baseQuery.Count(), 30, query.PageNumber);
        }
    }
}
