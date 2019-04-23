using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TesteJuntos.Domain.Interfaces.Service;
using TesteJuntos.Infra;
using TesteJuntos.Infra.Repositories;
using TesteJuntos.Service.Services;
using System.Data;
using TesteJuntos.Infra.Stores;
using TesteJuntos.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace TesteJuntos.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            

            //Account
            services.AddTransient<IAccountService, AccountService>();

            //Identity
            services.AddTransient<IUserStore<ApiUser>, UserStore>();
            services.AddTransient<IRoleStore<ApiRole>, RoleStore>();

            services.AddIdentity<ApiUser, ApiRole>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));
        }
    }
}
