using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TesteJuntos.Application.Secuity;
using TesteJuntos.Domain.AutoMapper;
using TesteJuntos.IoC;
using Newtonsoft.Json;
using AutoMapper;
using TesteJuntos.Application.Middleware;
using TesteJuntos.CrossCutting;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TesteJuntos.CrossCutting.Utils;
using TesteJuntos.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace TesteJuntos.Application
{
    public class Startup
    {
        private IHostingEnvironment enviroment;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();

            enviroment = env;

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConnectionStrings.TesteJuntosConnection = Configuration.GetConnectionString("TesteJuntosConnection");
            string Issuer = Configuration.GetSection("TokenConfigurations")["Issuer"];
            string Audience = Configuration.GetSection("TokenConfigurations")["Audience"];
            string Password = Configuration.GetSection("TokenConfigurations")["Password"];
            Environment.SetEnvironmentVariable("Issuer", Issuer);
            Environment.SetEnvironmentVariable("Audience", Audience);
            Environment.SetEnvironmentVariable("Password", Password);

            //Configure JTW token to work with Autorize
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(new Sha512(Password).ToString())) //Sign the JTW with a SHA512 hash of Password
                };
            });

            //services.AddIdentity<ApiUser, IdentityRole>()
            //         .AddDefaultTokenProviders();

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Formatting.Indented;
            });

            services.AddAuthorization(options =>
            {
                options.UseAuthorizationOptions();
            });

            Mapper.Initialize(cfg =>
            {
                cfg.ValidateInlineMaps = false;
            });

            services.AddAutoMapper(typeof(Startup).Assembly);
            AutoMapperConfiguration.RegisterMappings();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = $"Teste Junto API {enviroment.EnvironmentName}", Version = "v1", Description = "Projeto TesteJuntos" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" }); //Enable Bearer token o Swagger
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                { "Bearer", Enumerable.Empty<string>() },
            });
            }); //Emable Swagger on Application on /Swagger

            services.AddOptions();

            services.AddCors(o => o.AddPolicy("Cors", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins("http://localhost:3000");
            }));
            // Registrar todos os IoC
            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseResponseExceptionHandler();
            
            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                
            });

            app.UseCors();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });


        }

        private static void RegisterServices(IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}
