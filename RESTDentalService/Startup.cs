using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RESTDentalService.Entity;
using RESTDentalService.Middleware;
using RESTDentalService.Models;
using RESTDentalService.Services;
using RESTDentalService.Validators;

namespace RESTDentalService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<DentalRestDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("default"));
            });

            /* Auto-Mapper */
            services.AddAutoMapper(GetType().Assembly);

            /* Swagger */
            services.AddSwaggerGen();

            /* Fluent Validation */
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddScoped<IValidator<CreateClinicDTO>, CreateClinicDTOValidator>();
            services.AddScoped<IValidator<CreateEmployeeDTO>, CreateEmployeeDTOValidator>();
            services.AddScoped<IValidator<CreateOperationDTO>, CreateOperationDTOValidator>();
            services.AddScoped<IValidator<RegisterUserDTO>, RegisterUserDTOValidator>();

            /* Middlewares */
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();

            /* Services */
            services.AddScoped<IClinicService, ClinicService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IOperationService, OperationService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            /* CORS */
            services.AddCors(opt =>
            {
                opt.AddPolicy("frontend-client", builder =>
                {
                    builder.AllowAnyMethod()
                           .AllowAnyHeader()
                           .WithOrigins(Configuration["AllowedOrigins"]);
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dental API"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
