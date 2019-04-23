using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TesteJuntos.CrossCutting;
using TesteJuntos.IoC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using TesteJuntos.Domain.AutoMapper;
using TesteJuntos.Application;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TesteJuntos.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TesteJuntos.Test
{
    public class BaseTest
    {
        public ServiceProvider _serviceProvider { get; set; }
        public BaseTest()
        {

            ConnectionStrings.TesteJuntosConnection = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Cleverson\\Projetos\\TesteJuntos.API\\TestDB.mdf;Integrated Security=True;Connect Timeout=30";
            var services = new ServiceCollection();
            
            Mapper.Initialize(cfg =>
            {
                cfg.ValidateInlineMaps = false;
            });

            services.AddAutoMapper(typeof(Startup).Assembly);
            AutoMapperConfiguration.RegisterMappings();


            services.AddOptions();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            NativeInjectorBootStrapper.RegisterServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }
        public static bool ExistErrors(IActionResult actionResult)
        {
            BadRequestObjectResult badRequest = actionResult as BadRequestObjectResult;

            //Não tem erros
            if (badRequest == null)
                return false;

            var json = JsonConvert.SerializeObject(badRequest.Value);
            var response = JsonConvert.DeserializeObject<Response>(json);

            return (response.errors.Any());
        }

        public static bool Sucesso(IActionResult actionResult)
        {
            OkObjectResult okRequest = actionResult as OkObjectResult;

            //Tem erros
            if (okRequest == null)
                return false;

            var json = JsonConvert.SerializeObject(okRequest.Value);
            var response = JsonConvert.DeserializeObject<Response>(json);

            return response.success;
        }

        public class Response
        {
            public bool success { get; set; }
            public List<string> errors { get; set; }
        }
    }
}
