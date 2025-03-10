using Application.App.User.Command;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(mfg =>
            {
                //mfg.RegisterServicesFromAssemblyContaining<CreateUserCommand>();
                mfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            return services;
        }
    }
}
