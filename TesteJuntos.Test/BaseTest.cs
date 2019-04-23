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
            NativeInjectorBootStrapper.RegisterServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
