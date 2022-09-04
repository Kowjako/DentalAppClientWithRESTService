using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RESTDentalService.Entity;
using RESTDentalService.Middleware;
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
            services.AddAutoMapper(GetType().Assembly);
            services.AddSwaggerGen();

            /* Fluent Validation */
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddScoped<IValidator<CreateClinicDTO>, CreateClinicDTOValidator>();


            /* Middlewares */
            services.AddScoped<ErrorHandlingMiddleware>();

            /* Services */
            services.AddScoped<IClinicService, ClinicService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

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
