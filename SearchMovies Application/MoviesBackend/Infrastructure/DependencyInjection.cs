using App.Core.Interface;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using App.Core.Model;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
          IConfigurationManager configuration)
        {
            services.AddScoped<IAppDbContext, AppDbContext>();
            //services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IJwtService, JwtService>();
            //services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
            //services.AddScoped<IValidator<RegisterationDto>, RegisterationDtoValidator>();


            services.AddDbContext<AppDbContext>((provider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            });

            return services;
        }
    }
}
