using Application.Interface;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfigurationManager configuration)
        {


            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<IGeneratePassword, GeneratePasswordService>();

            services.AddDbContext<AppDbContext>((provider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            });
            return services;
        }
    }

}
